using Grocery.App.ViewModels;

namespace Grocery.App.Views
{
    public partial class ProductView : ContentPage
    {
        public ProductView(ProductViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing() // liet eerst nieuwe producten niet zien, nu wel omdat we elke keer de lijst refreshen bij het tonen
        {
            base.OnAppearing();
            (BindingContext as ProductViewModel)?.LoadProducts();
        }
    }
}
