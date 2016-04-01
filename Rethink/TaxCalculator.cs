namespace Rethink
{
    public class TaxCalculator : ITaxCalculator
	{
		public decimal CalculateTaxes (decimal subTotal)
		{
            return 0.1m;
		}
	}
}
