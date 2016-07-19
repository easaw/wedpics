using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace wedpics
{
	//
	//					v8images -- storage
	//

	public partial class wedpicsPage : ContentPage
	{
		public wedpicsPage()
		{
			this.InitializeComponent();

			takePhoto.Clicked += async (sender, args) =>
		   	{
				var file = await OnTakePhoto();
				await DisplayAlert("File Location", file.Path, "OK");

				ShowImageFile(file);
		   	};


			pickPhoto.Clicked += async (sender, args) =>
			{
				var file = await PickPhoto();

				await DisplayAlert("File Location", file.Path, "OK");
				ShowImageFile(file);
			};
		}

		private async Task<MediaFile> OnTakePhoto()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{

				DisplayAlert("No Camera", ":( No camera available.", "OK");
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg"
			});

			return file;
		}

		private async Task<MediaFile> PickPhoto()
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
				return null;
			}

			var file = await CrossMedia.Current.PickPhotoAsync();

			return file;
		}

		private void ShowImageFile(MediaFile file)
		{
			if (file == null)
			{
				return;
			}
			
			image.Source = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					file.Dispose();
					return stream;
				});
		}
	}
}

