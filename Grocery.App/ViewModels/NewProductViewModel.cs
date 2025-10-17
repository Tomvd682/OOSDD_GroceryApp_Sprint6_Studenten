using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : ObservableObject
    {
        private readonly IProductService _productService;

        [ObservableProperty] private string name = "";
        [ObservableProperty] private int stock;
        [ObservableProperty] private DateOnly shelfLife = DateOnly.FromDateTime(DateTime.Today.AddMonths(6));
        [ObservableProperty] private decimal price;

        [ObservableProperty] private bool canSave;

        public NewProductViewModel(IProductService productService)
        {
            _productService = productService;
            Recalc();
        }

        partial void OnNameChanged(string value) => Recalc();
        partial void OnStockChanged(int value) => Recalc();
        partial void OnShelfLifeChanged(DateOnly value) => Recalc();
        partial void OnPriceChanged(decimal value) => Recalc();

        private void Recalc()
        {
            CanSave = !string.IsNullOrWhiteSpace(Name) && Stock >= 0 && Price >= 0m;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (!CanSave) return;

            var p = new Product(0, Name.Trim(), Stock, ShelfLife, Price);
            _productService.Add(p);

            // Terug naar het vorige scherm; ProductView refresht in OnAppearing()
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
