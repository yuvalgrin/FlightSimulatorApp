using System;


interface Command
{
	public void execute(String[] args);
}

public class Set : Command
{
	public Set()
	{
	}

	public void execute(String[] args) { return; }
}


public class Get : Command
{
	public Get()
    {

    }

	public void execute(String[] args) { return; }

}