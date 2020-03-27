using System;


public abstract class Command
{
	public abstract String execute();

	public static Command parseRawCommand(String rawCmd)
	{
		try
		{
			String[] cmdArr = rawCmd.Split(' ');
			if (cmdArr[0].Equals("set"))
			{
				String param = cmdArr[1];
				Double value = Double.Parse(cmdArr[2]);
				return new SetCommand(param, value);
			}
			else if (cmdArr[0].Equals("get"))
			{
				String param = cmdArr[1];
				Double value = Double.Parse(cmdArr[2]);
				return new GetCommand(param, value);
			}
		} catch (Exception ex)
		{
			// Error
		}

		return null;
	}
}

public class SetCommand : Command
{
	double value = 0;
	String param = "";

	public SetCommand(String param, double value)
	{
		this.value = value;
		this.param = param;
	}

	public override String execute() { return "get " + param + " " + value; }
}


public class GetCommand : Command
{
	double value = 0;
	String param = "";

	public GetCommand(String param, double value)
    {
		this.value = value;
		this.param = param;
	}

	public override String execute() { return "get " + param + " " + value; }

}