nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"vstest.console.exe" -filter:"+[Konves.TextGraph*]* -[*.*Tests]*" -excludebyfile:"*.g.cs" -targetargs:".\tests\Konves.TextGraph.UnitTests\bin\Release\Konves.TextGraph.UnitTests.dll .\tests\Konves.TextGraph.HtmlParser.UnitTests\bin\Release\Konves.TextGraph.HtmlParser.UnitTests.dll .\tests\Konves.TextGraph.MarkdownParser.UnitTests\bin\Release\Konves.TextGraph.MarkdownParser.UnitTests.dll" -register:user
.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml
