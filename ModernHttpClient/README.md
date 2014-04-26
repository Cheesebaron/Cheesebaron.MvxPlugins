ModernHttpClient
================

Wraps [Paul Betts's](https://github.com/paulcbetts) [ModernHttpClient](https://github.com/paulcbetts/ModernHttpClient) into a MvvmCross plugin.

Instructions
============

1. Grab the [latest ModernHttpClient binaries](https://github.com/paulcbetts/ModernHttpClient/releases) or build from source and extract into the vendor folder.
2. Build
3. ???
4. Profit

All necessary dll's will be output in the `bin` folder in the root of this repository. So for Android, remember to reference OkHttp from you UI project, otherwise you will get an Exception saying that OkHttp is not found.
