c:
cd \WINDOWS\Microsoft.NET\Framework\v2.0.50727\
RegAsm.exe "E:\Dev\Examples\Snarl styles\myFirstCsharpSnarlStyle\myFirstCsharpSnarlStyle\myFirstCsharpSnarlStyle\bin\Release\twitterSnarlStyle.dll" /verbose /codebase
rem  /tlb:myFirstCsharpSnarlStyle.dll
rem "C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\gacutil.exe" /i myFirstCsharpSnarlStyle.dll
rem "C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\gacutil.exe" /i myFirstCsharpSnarlStyle.dll

pause

RegAsm.exe "E:\Dev\Examples\Snarl styles\myFirstCsharpSnarlStyle\myFirstCsharpSnarlStyle\myFirstCsharpSnarlStyle\bin\Release\twitterSnarlStyle.dll" /unregister
e:
pause