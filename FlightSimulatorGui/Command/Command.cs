using System;


interface Command
{
	void execute(String[] args);
}

public class SetCommand : Command
{
	public SetCommand()
	{
	}

	public void execute(String[] args) { return; }
}


public class GetCommand : Command
{
	public GetCommand()
    {

    }

	public void execute(String[] args) { return; }

}