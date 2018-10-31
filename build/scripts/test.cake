///////////////////////////////////////////////////////////////////////////////
// TEST
///////////////////////////////////////////////////////////////////////////////



Task("Run-Unit-Tests")
	.WithCriteria(() => (target != "Skip-Test") && (target != "Skip-Restore"))
    .IsDependentOn("Build")
    .Does(() =>
{
	// Run Test
    foreach(string test in testNames)
    {
		Information("Running unit tests: {0}", test);

		string outputPath = testResultsDir + "/" + test.Replace(".Tests", "") + ".xml";
		outputPath = MakeAbsolute(File(outputPath)).FullPath;
		
        DotNetCoreTest("./src/" + test + "/" + test + ".csproj", new DotNetCoreTestSettings
        {
			NoRestore = true,
            ArgumentCustomization = args => args.AppendSwitch("-a", " ", ".".Quote())
												.AppendSwitch("-l", " ", ("xunit;LogFilePath=" + outputPath).Quote())
        });
    }



	// Build Report
    Information("Building report");

    if (testNames.Count > 0)
    {
        ReportUnit(testResultsDir);
    }
})
.OnError(exception =>
{
	// Get Errors
	IList<string> errors = new List<string>();

	foreach(string test in testNames)
    {
		IList<XunitResult> testResults = GetXunitResults(testResultsDir + "/" + test.Replace(".Tests", "") + ".xml");
		
		foreach(XunitResult testResult in testResults)
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



	// Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if (string.IsNullOrEmpty(token))
    {
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



	// Post Message
	SlackChatMessageResult result;
	
	SlackChatMessageSettings settings = new SlackChatMessageSettings()
	{
		Token = token,
		UserName = "Cake",
		IconUrl = new System.Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png")
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
    //Load Document
    XDocument doc = XDocument.Load(filePath);



    //Get Podcasts
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