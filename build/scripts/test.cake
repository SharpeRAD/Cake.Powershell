///////////////////////////////////////////////////////////////////////////////
// TEST
///////////////////////////////////////////////////////////////////////////////

Task("Run-Unit-Tests")
    .WithCriteria(() => (target != "Skip-Test") && (target != "Skip-Restore"))
    .IsDependentOn("Build")
    .Does(() =>
{
    // Run Test
    foreach (string test in testNames)
    {
        Information("Running unit tests: {0}", test);

        if (isRunningOnTravis)
        {
            string outputPath1 = testResultsDir + "/" + test.Replace(".Tests", "") + ".6.0.xml";
            outputPath1 = MakeAbsolute(File(outputPath1)).FullPath;
            DotNetTest("./src/" + test + "/" + test + ".csproj", new DotNetTestSettings
            {
                NoRestore = true,
                Framework = "net6.0",
                ArgumentCustomization = args => args.AppendSwitch("-l", " ", ("xunit;LogFilePath=" + outputPath1).Quote())
            });

            string outputPath2 = testResultsDir + "/" + test.Replace(".Tests", "") + ".7.0.xml";
            outputPath2 = MakeAbsolute(File(outputPath2)).FullPath;
            DotNetTest("./src/" + test + "/" + test + ".csproj", new DotNetTestSettings
            {
                NoRestore = true,
                Framework = "net7.0",
                ArgumentCustomization = args => args.AppendSwitch("-l", " ", ("xunit;LogFilePath=" + outputPath2).Quote())
            });

            string outputPath3 = testResultsDir + "/" + test.Replace(".Tests", "") + ".8.0.xml";
            outputPath3 = MakeAbsolute(File(outputPath3)).FullPath;
            DotNetTest("./src/" + test + "/" + test + ".csproj", new DotNetTestSettings
            {
                NoRestore = true,
                Framework = "net8.0",
                ArgumentCustomization = args => args.AppendSwitch("-l", " ", ("xunit;LogFilePath=" + outputPath3).Quote())
            });
        }
        else
        {
            string outputPath = testResultsDir + "/" + test.Replace(".Tests", "") + ".xml";
            outputPath = MakeAbsolute(File(outputPath)).FullPath;
            DotNetTest("./src/" + test + "/" + test + ".csproj", new DotNetTestSettings
            {
                NoRestore = true,
                ArgumentCustomization = args => args.AppendSwitch("-l", " ", ("xunit;LogFilePath=" + outputPath).Quote())
            });
        }
    }



    // Build Report
    Information("Building report");

    if (testNames.Count > 0)
    {
        Information(testResultsDir);
        ReportUnit(testResultsDir);
    }
})
.OnError(exception =>
{
    // Get Errors
    IList<string> errors = new List<string>();

    foreach (string test in testNames)
    {
        IList<XunitResult> testResults = GetXunitResults(testResultsDir + "/" + test.Replace(".Tests", "") + ".xml");

        foreach (XunitResult testResult in testResults)
        {
            errors.Add(testResult.Type + " => " + testResult.Method);
            errors.Add(testResult.StackTrace);
        }
    }

    if (errors.Count == 0)
    {
        errors.Add(exception.Message);
    }



    // Get Message
    var title = "Unit-Tests failed for " + appName + " v" + version;
    var text = "";

    for (int index = 0; index < errors.Count; index++)
    {
        text += errors[index];

        if (index < (errors.Count - 1))
        {
            text += "\n";
        }
    }



    // Resolve the webHook Url.
    var webHookUrl = EnvironmentVariable("SLACK_WEBHOOK_URL");

    if (string.IsNullOrEmpty(webHookUrl))
    {
        throw new InvalidOperationException("Could not resolve Slack webHook Url.");
    }



    // Post Message
    SlackChatMessageResult result;

    SlackChatMessageSettings settings = new SlackChatMessageSettings()
    {
        IncomingWebHookUrl = webHookUrl,
        UserName = "Cake",
        IconUrl = new System.Uri("https://cdn.jsdelivr.net/gh/cake-build/graphics/png/cake-small.png")
    };

    IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();

    attachments.Add(new SlackChatMessageAttachment()
    {
        Color = "danger",
        Text = text
    });

    result = Slack.Chat.PostMessage("#code", title, attachments, settings);



    // Check Result
    if (result.Ok)
    {
        // Posted
        Information("Message was succcessfully sent to Slack.");
    }
    else
    {
        // Error
        Error("Failed to send message to Slack: {0}", result.Error);
    }

    throw exception;
});





using System.Xml.Linq;

public IList<XunitResult> GetXunitResults(string filePath)
{
    // Load Document
    XDocument doc = XDocument.Load(filePath);
    IList<XElement> elements = doc.Descendants("test").Where(e => e.Attribute("result").Value == "Fail").ToList();
    IList<XunitResult> results = new List<XunitResult>();

    foreach (XElement element in elements)
    {
        XAttribute type = element.Attribute("type");
        XAttribute method = element.Attribute("method");

        XElement file = element.Descendants("source-file").FirstOrDefault();
        XElement line = element.Descendants("source-line").FirstOrDefault();

        XElement message = element.Descendants("message").FirstOrDefault();
        XElement stackTrace = element.Descendants("stack-trace").FirstOrDefault();

        results.Add(new XunitResult()
        {
            Type = type != null ? type.Value : "",
            Method = method != null ? method.Value : "",

            File = file != null ? file.Value : "",
            Line = line != null ? line.Value : "",

            Message = message != null ? message.Value : "",
            StackTrace = stackTrace != null ? stackTrace.Value : "",
        });
    }

    return results;
}



public class XunitResult
{
    public string Type { get; set; }

    public string Method { get; set; }



    public string File { get; set; }

    public string Line { get; set; }



    public string Message { get; set; }

    public string StackTrace { get; set; }
}