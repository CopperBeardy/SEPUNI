namespace CordEstates.Models.DTOs
{
    public class AddressDTO
    {

        public int Id { get; set; }
        public string Number { get; set; }


        public string FirstLine { get; set; }

        public string SecondLine { get; set; }

        public string TownCity { get; set; }

        public string Postcode { get; set; }
    }
}
