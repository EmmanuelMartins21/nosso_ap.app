namespace nosso_apartamento
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
        }

        protected override  bool OnBackButtonPressed()
        {
            if (Shell.Current.Navigation.NavigationStack.Count > 1)
            {
                Shell.Current.GoToAsync("..");
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}
