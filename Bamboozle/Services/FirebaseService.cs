using Firebase.Xamarin.Database;

namespace Bamboozle
{
	public static class FirebaseService
	{
		static FirebaseClient _client;
		static FirebaseService()
		{
			_client = new FirebaseClient("https://bamboozle-c07fb.firebaseio.com/");
		}
		public static FirebaseClient Client
		{
			get { return _client; }
		}
	}
}