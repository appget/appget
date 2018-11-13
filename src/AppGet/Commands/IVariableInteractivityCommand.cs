namespace AppGet.Commands
{
    public interface IVariableInteractivityCommand
    {
        bool Interactive { get; set; }
        bool Silent { get; set; }
        bool Passive { get; set; }
    }
}