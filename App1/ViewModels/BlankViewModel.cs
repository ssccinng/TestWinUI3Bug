using CommunityToolkit.Mvvm.ComponentModel;
using S7PLCSimulator;
namespace App1.ViewModels;

public class BlankViewModel : ObservableRecipient
{
    public SimpleSiemensS7Server SimpleSiemensS7Server = new();
    public BlankViewModel()
    {
        
    }

    public void Start()
    {
        SimpleSiemensS7Server.Start();
    }
}
