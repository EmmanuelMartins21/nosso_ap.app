using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace nosso_apartamento.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        
        [RelayCommand]
        private async Task GoToListaCompras()
        {
            //await AppShell.Current.GoToAsync("listacompras");
            await Shell.Current.GoToAsync("//listacompras");
        }
    }
}
