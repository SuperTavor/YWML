# YWML
An efficient mod loader for Yo-kai Watch written in C#.

## Limitations
The mod loader currently only supports YKW1 Europe. More regions and games will be added in the very near future.

Adding multiple, non conflicting mods is currently not possible, but it will be in the near future.

## Usage
to pack a modded romfs in the YWML format, run

`ywml create [file path to modded romfs] [mod name] [mod version] [mod author]`

to load a mod from a `ywm` file, run

`ywml load [file path to ywm file]`
