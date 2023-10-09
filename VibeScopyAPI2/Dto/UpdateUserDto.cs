namespace VibeScopyAPI.Dto
{
	public class UpdateUserPhotoDto
	{
		public string FbId { get; set; }

        public ICollection<string> AWSS3Path { get; set; }
    }
}

