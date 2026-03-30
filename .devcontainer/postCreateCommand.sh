#!/bin/sh
echo "USER:" `whoami`

# add xunit3 template
dotnet new install xunit.v3.templates
