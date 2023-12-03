# YWML
An efficient mod loader for Yo-kai Watch written in C#.

## Supported games:
* Yo-kai Watch 1 (Europe) For 3DS
### More will be added in the near future.

## Limitations
Music and Video mods are not supported currently, they will simply be ignored by the packer. Will fix asap
## Usage
to pack a modded romfs in the YWML format, run

`ywml create [file path to modded romfs] [mod name] [mod version] [mod author]`

to load a mod from a `ywm` file, run

`ywml load [file path to ywm file]`
