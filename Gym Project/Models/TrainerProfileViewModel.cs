namespace Gym_Project.Models
{
    public class TrainerProfileViewModel
    {
        public int Id { get; set; }


        public string FullName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string FavouriteExercise { get; set; }=string.Empty;

        public string Slogan { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public string? Education { get; set; }
    }
}
