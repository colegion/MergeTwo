namespace Currencies
{
    public class Coin : Currency
    {
        protected override void Start()
        {
            _saveKey = "coins";
            base.Start();
        }
    }
}
