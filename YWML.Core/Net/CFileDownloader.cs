namespace YWML.Src.Net
{
    public static class CFileDownloader
    {
        private const int PARALLEL_MIN_BYTES = 8 * 1024 * 1024;
        private const int MAX_CHUNKS = 4;
        private const int BUFFER_SIZE = 1 << 20;

        public static async Task DownloadAsync(string link, string destinationPath, IProgress<int>? percent, HttpMessageHandler? handler = null)
        {
            using var client = handler == null ? new HttpClient() : new HttpClient(handler);

            var probe = await client.GetAsync(link, HttpCompletionOption.ResponseHeadersRead);
            probe.EnsureSuccessStatusCode();

            var totalBytes = probe.Content.Headers.ContentLength ?? 0;
            var supportsRanges = probe.Headers.AcceptRanges.Contains("bytes");

            if (totalBytes >= PARALLEL_MIN_BYTES && supportsRanges)
            {
                probe.Dispose();
                try
                {
                    await DownloadParallelAsync(client, link, destinationPath, totalBytes, percent);
                }
                catch
                {
                    //Server didn't honor the ranges, or a chunk failed - redo it sequentially.
                    await DownloadSequentialAsync(client, link, destinationPath, percent);
                }
            }
            else
            {
                using (probe)
                {
                    await WriteResponseToFileAsync(probe, destinationPath, totalBytes, percent);
                }
            }

            var length = new FileInfo(destinationPath).Length;
            if (totalBytes > 0 && length != totalBytes)
            {
                throw new IOException("The download was incomplete.");
            }
        }

        private static async Task DownloadParallelAsync(HttpClient client, string link, string path, long totalBytes, IProgress<int>? percent)
        {
            var chunkCount = (int)Math.Min(MAX_CHUNKS, Math.Max(1, totalBytes / PARALLEL_MIN_BYTES));
            var chunkSize = totalBytes / chunkCount;
            var downloadedPerChunk = new long[chunkCount];

            using (var allocation = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write, BUFFER_SIZE, true))
            {
                allocation.SetLength(totalBytes);
            }

            void Report()
            {
                if (percent == null)
                {
                    return;
                }

                long sum = 0;
                for (var i = 0; i < downloadedPerChunk.Length; i++)
                {
                    sum += Interlocked.Read(ref downloadedPerChunk[i]);
                }

                percent.Report((int)(sum * 100 / totalBytes));
            }

            var tasks = new Task[chunkCount];
            for (var i = 0; i < chunkCount; i++)
            {
                var index = i;
                var start = (long)i * chunkSize;
                var end = i == chunkCount - 1 ? totalBytes - 1 : start + chunkSize - 1;

                tasks[i] = Task.Run(async () =>
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, link);
                    request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);

                    using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode != System.Net.HttpStatusCode.PartialContent)
                    {
                        throw new InvalidOperationException("The server did not honor the range request.");
                    }

                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var file = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write, BUFFER_SIZE, true);
                    file.Seek(start, SeekOrigin.Begin);

                    var buffer = new byte[BUFFER_SIZE];
                    var remaining = end - start + 1;

                    while (remaining > 0)
                    {
                        var read = await stream.ReadAsync(buffer.AsMemory(0, (int)Math.Min(buffer.Length, remaining)));
                        if (read <= 0)
                        {
                            break;
                        }

                        await file.WriteAsync(buffer.AsMemory(0, read));
                        remaining -= read;
                        Interlocked.Add(ref downloadedPerChunk[index], read);
                        Report();
                    }

                    if (remaining != 0)
                    {
                        throw new IOException("The download was incomplete.");
                    }
                });
            }

            await Task.WhenAll(tasks);
        }

        private static async Task DownloadSequentialAsync(HttpClient client, string link, string path, IProgress<int>? percent)
        {
            using var response = await client.GetAsync(link, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var totalBytes = response.Content.Headers.ContentLength ?? 0;
            await WriteResponseToFileAsync(response, path, totalBytes, percent);
        }

        private static async Task WriteResponseToFileAsync(HttpResponseMessage response, string path, long totalBytes, IProgress<int>? percent)
        {
            using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, BUFFER_SIZE, true);
            using var stream = await response.Content.ReadAsStreamAsync();

            var buffer = new byte[BUFFER_SIZE];
            long downloaded = 0;
            int read;

            while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
            {
                await file.WriteAsync(buffer.AsMemory(0, read));
                downloaded += read;

                if (totalBytes > 0)
                {
                    percent?.Report((int)(downloaded * 100 / totalBytes));
                }
            }
        }
    }
}
