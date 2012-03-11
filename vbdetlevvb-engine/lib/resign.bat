call "%VS110COMNTOOLS%vsvars32.bat"
C:\Windows\Microsoft.NET\Framework\v4.0.30319\ildasm /all /out=%1.il %1.dll
C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm /dll /key=....actvalue.snk %1.il
pause