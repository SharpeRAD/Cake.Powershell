///////////////////////////////////////////////////////////////////////////////
// MESSAGE
///////////////////////////////////////////////////////////////////////////////

Task("Slack")
	.WithCriteria(() => !isPullRequest)
    .Does(() =>
{
    // Resolve the API key.
    var token = EnvironmentVariable("SLACK_TOKEN");

    if (string.IsNullOrEmpty(token))
    {
        throw new InvalidOperationException("Could not resolve Slack token.");
    }



    // Attachments
    var text = "";
	var packages = false;
	
	for (int index = 0; index < projectNames.Count; index++)
	{
		if (projectNames[index] != appName)
		{
			text += projectNames[index] + "." + version + ".nupkg";
			packages = true;
		}
		
		if (index < (projectNames.Count - 1))
		{
			text += "\n";
		}
	}
	
	
	
	// Title
	var title = "";
	
	if (packages)
	{
		title = "Published <https://www.nuget.org/packages/" + appName + "/|" + appName + " v" + version + ">";
	}
	else
	{
		title = "Published " + appName + " v" + version;
	}
	
	
	
	// Post Message
	SlackChatMessageResult result;
	
	SlackChatMessageSettings settings = new SlackChatMessageSettings()
	{
		Token = token,
		UserName = "Cake",
		IconUrl = new System.Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png")
	};

	if (packages)
	{
		IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();
		
		attachments.Add(new SlackChatMessageAttachment()
		{
			Color = "good",
			Text = text
		});
		
		result = Slack.Chat.PostMessage("#code", title, attachments, settings);
	}
	else
	{
		result = Slack.Chat.PostMessage("#code", title, settings);
	}

	
	
	// Check Result
    if (result.Ok)
    {
        //Posted
        Information("Message was succcessfully sent to Slack.");
    }
    else
    {
        //Error
        Error("Failed to send message to Slack: {0}", result.Error);
    }
});