#!/bin/bash
clear
msbuild Tests.sln -property:Configuration=Release -verbosity:quiet 
mono bin/Debug/net6.0/Tests $@
