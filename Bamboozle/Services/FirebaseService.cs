using Firebase.Xamarin.Database;

namespace Bamboozle
{
	public static class FirebaseService
	{
		static FirebaseClient _client;
		public static string Photo { get { return "https://firebasestorage.googleapis.com/v0/b/bamboozle-c07fb.appspot.com/o/Images%2Ftestphoto?alt=media&token=1529e434-7bb5-44ca-ac81-0c2082c391d4"; } }
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