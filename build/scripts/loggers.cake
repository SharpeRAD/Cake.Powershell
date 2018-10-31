///////////////////////////////////////////////////////////////////////////////
// JUNIT
///////////////////////////////////////////////////////////////////////////////

using System.Xml.Linq;

public IList<JunitSuite> GetJunitResults(string filePath)
{
    // Load Document
    XDocument doc = XDocument.Load(filePath);



    // Get Suites
    IList<XElement> lstSuite = doc.Descendants("testsuite").ToList();
    IList<JunitSuite> results = new List<JunitSuite>();

    foreach (XElement itmSuite in lstSuite)
    {
        XAttribute package = itmSuite.Attribute("package");
        XAttribute name = itmSuite.Attribute("name");
		XAttribute time = itmSuite.Attribute("time");

		JunitSuite suite = new JunitSuite()
        {
            Package = package != null ? package.Value : "",
            Name = name != null ? name.Value : "",
            Time = time != null ? Convert.ToInt32(time.Value) : 0,

			Cases = new List<JunitCase>()
        };



		// Get Name
		string prefix = GetPrefix(suite.Name);

		if (!String.IsNullOrEmpty(prefix))
		{
			suite.Name = suite.Name.Replace(prefix, "");
		}

		suite.Name = suite.Name.Replace(@"\", "/");



		// Get Cases
		IList<XElement> lstCase = itmSuite.Descendants("testcase").ToList();
		
		foreach (XElement itmCase in lstCase)
		{
			// Create Test
			name = itmCase.Attribute("name");
			time = itmCase.Attribute("time");

			JunitCase test = new JunitCase()
			{
				Name = name != null ? name.Value : "",
				Time = time != null ? Convert.ToInt32(time.Value) : 0,

				Failures = new List<JunitResult>(),
				Errors = new List<JunitResult>()
			};



			// Get Errors
			IList<XElement> lstError = itmCase.Descendants("error").ToList();

			foreach (XElement itmError in lstError)
			{
				XAttribute message = itmError.Attribute("message");

				test.Errors.Add(GetJunitResult(name != null ? name.Value : "", message != null ? message.Value : "", itmError.Value, filePath.EndsWith("js.xml") ? "JS" : "CSS"));
			}



			// Get Failures
			IList<XElement> lstFailure = itmCase.Descendants("failure").ToList();

			foreach (XElement itmFailure in lstFailure)
			{
				XAttribute message = itmFailure.Attribute("message");

				test.Failures.Add(GetJunitResult(name != null ? name.Value : "", message != null ? message.Value : "", itmFailure.Value, filePath.EndsWith("js.xml") ? "JS" : "CSS"));
			}



			// Add Test
			if ((test.Errors.Count > 0) || (test.Failures.Count > 0))
			{
				suite.Cases.Add(test);
			}
		}

        results.Add(suite);
    }
            
    return results;
}



public JunitResult GetJunitResult(string name, string message, string inner, string type)
{
	inner = inner.Replace("<![CDATA[", "").Replace("]]>", "");
	string location = "";

	if (type == "JS")
	{
		name = name.Replace("org.eslint.", "");
		inner = inner.Replace(" (" + name + ")", "");

		int start = inner.IndexOf(", Error - ");

		if (start > 0)
		{
			location = inner.Substring(0, start);
			location = location.Replace("line ", "(").Replace(", col ", ", ") + ")";

			start = start + 10;
			inner = inner.Substring(start, inner.Length - start);
		}
	}
	else
	{
		// Inner
		int start = inner.IndexOf(":");

		if (start > 0)
		{
			// Line
			string line = inner.Substring(0, start);
			inner = inner.Substring(start, inner.Length - start);

			if (inner.Length > 1)
			{
				inner = inner.Substring(1, inner.Length - 1);
			}



			// Column
			start = inner.IndexOf(":");

			if (start > 0)
			{
				string column = inner.Substring(0, start);
				inner = inner.Substring(start, inner.Length - start);
		
				if (inner.Length > 1)
				{
					inner = inner.Substring(1, inner.Length - 1);
				}

				location = "(" + line.Replace(":", "") + ", " + column.Replace(":", "") + ")";
			}
		}
	}

	//Warning("LOCATION === " + location);
	//Warning("INNER === " + inner);

	return new JunitResult()
	{
		Message = message.Trim(),

		Source = inner.Trim(),
		Location = location.Trim()
	};
}



public IList<SlackChatMessageAttachment> GetJunitAttachments(IList<JunitSuite> lstSuite)
{
	IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();

	foreach(JunitSuite itmSuite in lstSuite)
	{
		foreach(JunitCase itmCase in itmSuite.Cases)
		{
			// Errors
			foreach(JunitResult itmResult in itmCase.Errors)
			{
				attachments.Add(new SlackChatMessageAttachment()
				{
					Pretext = itmSuite.Name.Trim(),
					Title = itmResult.Message.Trim(),
					Text = itmResult.Location.Trim(),
					Color = "danger"
				});
			}

			// Warnings
			foreach(JunitResult itmResult in itmCase.Failures)
			{
				attachments.Add(new SlackChatMessageAttachment()
				{
					Pretext = itmSuite.Name.Trim(),
					Title = itmResult.Message.Trim(),
					Text = itmResult.Location.Trim(),
					Color = "warning"
				});
			}
		}
	}

	return attachments;
}



public class JunitSuite
{
    public string Package { get; set; }

    public string Name { get; set; }


	
    public int Time { get; set; }

    public int Errors { get; set; }

    public int Failures { get; set; }



    public IList<JunitCase> Cases { get; set; }
}

public class JunitCase
{
    public string Name { get; set; }
	
    public int Time { get; set; }

	

    public IList<JunitResult> Failures { get; set; }
	
    public IList<JunitResult> Errors { get; set; }
}

public class JunitResult
{
    public string Message { get; set; }
	
    public string Source { get; set; }

	public string Location { get; set; }
}





///////////////////////////////////////////////////////////////////////////////
// ATTACHMENTS
///////////////////////////////////////////////////////////////////////////////

public void CombineAttachments(IList<SlackChatMessageAttachment> lstOutput, IList<SlackChatMessageAttachment> lstInput)
{
	foreach(SlackChatMessageAttachment itmAttachment in lstInput)
	{
		if (lstOutput.Count < 10)
		{
			if ((lstOutput.Count > 0) && (lstOutput[lstOutput.Count - 1].Pretext == itmAttachment.Pretext))
			{
				itmAttachment.Pretext = "";
			}

			lstOutput.Add(itmAttachment);
		}
	}
}

public string GetPrefix(string name)
{
	string prefix = "";

	foreach(string projectName in projectNames)
	{
		int start = name.IndexOf(projectName);

		if (start > 0)
		{
			prefix = name.Substring(0, start + projectName.Length + 1);
		}
	}
	
	foreach(string testName in testNames)
	{
		int start = name.IndexOf(testName);

		if (start > 0)
		{
			prefix = name.Substring(0, start + testName.Length + 1);
		}
	}

	return prefix;
}





///////////////////////////////////////////////////////////////////////////////
// MS BUILD
///////////////////////////////////////////////////////////////////////////////

public IList<SlackChatMessageAttachment> GetMsBuildAttachments(string path, Exception exception)
{
	// Get Errors
	IList<SlackChatMessageAttachment> attachments = new List<SlackChatMessageAttachment>();

	string[] lines = FileReadLines(path);
	IList<string> errors = new List<string>();

	foreach(string line in lines)
	{
		bool found = false;

		foreach(string name in projectNames)
		{
			if (line.StartsWith("  " + name + " -> "))
			{
				found = true;
			}
		}

		foreach(string name in testNames)
		{
			if (line.StartsWith("  " + name + " -> "))
			{
				found = true;
			}
		}

		if (!found)
		{
			errors.Add(line);
		}
	}



	// Get Attachments
	if (errors.Count > 0)
	{
		foreach(string error in errors)
		{
			int start = error.LastIndexOf("[");

			string name = "";
			string file = "";
			string source = "";
			string location = "";

			if (start > 0)
			{
				// Get Name
				name = error.Substring(start + 1, error.Length - (start + 2));
				string prefix = GetPrefix(name);

				if (!String.IsNullOrEmpty(prefix))
				{
					name = name.Replace(prefix, "");
				}

				name = name.Replace(@"\", "/").Replace(".csproj", "");

				source = error.Substring(0, start);
				int seperator = source.IndexOf(":");

				if (seperator > 0)
				{
					// Location
					file = source.Substring(0, seperator);
					start = file.LastIndexOf("(");
					
					if (start > 0)
					{
						location = file.Substring(start, file.Length - start);
						location = location.Replace(",", ", ");

						file = file.Substring(0, start);
					}



					// Source
					source = source.Substring(seperator, source.Length - seperator);

					if (source.StartsWith(": "))
					{
						source = source.Substring(2, source.Length - 2);
					}
				}
			}



			//Warning("PRETEXT === " + name + "/" + file);
			//Warning("TITLE === " + source);
			//Warning("TEXT === " + location);

			// Add Attachment
			attachments.Add(new SlackChatMessageAttachment()
			{
				Pretext = name.Trim() + "/" + file.Trim(),
				Title = source.Trim(),
				Text = location.Trim(),
				Color = "danger"
			});
		}
	}
	else
	{
		// General Error
		attachments.Add(new SlackChatMessageAttachment()
		{
			Pretext = "An exception occured while trying to run MSBuild",
			Text = exception.Message,
			Color = "danger"
		});
	}

	return attachments;
}