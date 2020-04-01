using System;
using FlightSimulatorGui.Model;
public abstract class Command
{
	public abstract String execute();
	public abstract string getValue();
	public abstract string path();

	public static Command parseRawCommand(String rawCmd)
	{
		try
		{
			String[] cmdArr = rawCmd.Split(' ');
			if (cmdArr[0].Equals("set") && cmdArr.Length == 3)
			{
				String param = cmdArr[1];
				Double value = Double.Parse(cmdArr[2]);
				return new SetCommand(param, value);
			}
			else if (cmdArr[0].Equals("get") && cmdArr.Length == 2)
			{
				String param = cmdArr[1];
				return new GetCommand(param);
			}
		} catch (Exception ex)
		{
			// Error
			// Send bad command was sent
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

	public override string path()
	{
		return this.param;
	}

	public override string getValue()
	{
		return this.value + "";
	}

	public override String execute() { return "get " + param + " " + value; }
}


public class GetCommand : Command
{
	String param = "";

	public GetCommand(String param)
    {
		this.param = param + "\n";
	}

	public override String execute() { return "get " + param; }

	public override string getValue()
	{
		return FlightSimulatorModel.get().getDataByKey(path());
	}

	public override string path()
	{
		return this.param.Substring(0, this.param.Length - 1); ;
	}

}