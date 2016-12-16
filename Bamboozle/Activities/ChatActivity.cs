using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Xamarin.Database;
using Firebase.Database;
using Firebase.Xamarin.Database.Query;
using Firebase.Auth;
using Plugin.Media;
using Firebase.Storage;
using Android.Gms.Tasks;
using Java.Lang;
using Android.Speech;
using Android.Content;

namespace Bamboozle
{
	[Activity(Label = "Chat", Theme = "@android:style/Theme.Holo.Light", Icon = "@drawable/icon")]
	public class ChatActivity : Activity, IValueEventListener, IOnSuccessListener
	{
		private List<MessageContent> _messageList = new List<MessageContent>();
		private FirebaseClient firebase;
		private ListView lstMessages;
		private Button btnSendMessage;
		private Button btnAddPhoto;
		private EditText txtMessage;
		private StorageReference storageReference;
		private string photoUrl = "none";
		private string chatKey;
		public int MyResultCode = 1;
		private readonly int VOICE = 10;
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Chat);
			firebase = FirebaseService.Client;
			btnSendMessage = FindViewById<Button>(Resource.Id.btnSendMessage);
			btnAddPhoto = FindViewById<Button>(Resource.Id.btnAddMessage);
			txtMessage = FindViewById<EditText>(Resource.Id.txtMessage);
			lstMessages = FindViewById<ListView>(Resource.Id.lstMessages);
			chatKey = Intent.GetStringExtra("chatkey") ?? "Data not available"; ;
			FirebaseDatabase.Instance.GetReference("messages").Child(chatKey).AddValueEventListener(this);
			btnSendMessage.Click += delegate
			{
				PostMessage();
				txtMessage.Text="";
			};
			btnSendMessage.LongClick += delegate
			  {
				  var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt,"speak now");
				  voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
				  voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
				  StartActivityForResult(voiceIntent, VOICE);
			  };


			btnAddPhoto.Click += delegate
			{
				AddPhoto();
			};
		}
		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						string textInput = txtMessage.Text + matches[0];

						// limit the output to 500 characters
						if (textInput.Length > 500)
							textInput = textInput.Substring(0, 500);
						txtMessage.Text = textInput;
					}
					else
						txtMessage.Text = "";
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters
// limit the output to 500 characters

				}
			}

			base.OnActivityResult(requestCode, resultVal, data);
		}

		private async void AddPhoto()
		{
			var file = await CrossMedia.Current.PickPhotoAsync();
			var stream = file.GetStream();
			file.Dispose();
			storageReference = FirebaseStorage.Instance.GetReference("Images").Child(Guid.NewGuid().ToString());
			storageReference.PutStream(stream).AddOnSuccessListener(this);

			//photoUrl = FirebaseService.Photo;
		}
		private async void PostMessage()
		{
			var item = await firebase.Child("messages").Child(chatKey).PostAsync(new MessageContent(FirebaseAuth.Instance.CurrentUser.Email, txtMessage.Text, photoUrl));
			photoUrl = "none";
		}
		private async void DisplayChatMessage()
		{
			_messageList.Clear();

			var items = await firebase.Child("messages").Child(chatKey)
				.OnceAsync<MessageContent>();

			foreach (var item in items)
				_messageList.Add(item.Object);
			MessageAdapter chatAdapter = new MessageAdapter(this, _messageList);
			lstMessages.Adapter = chatAdapter;
			lstMessages.StackFromBottom = true;
		}

		public void OnCancelled(DatabaseError error)
		{
			throw new NotImplementedException();
		}

		public void OnDataChange(DataSnapshot snapshot)
		{
			DisplayChatMessage();
		}

		public void OnSuccess(Java.Lang.Object result)
		{
			UploadTask.TaskSnapshot res = result as UploadTask.TaskSnapshot;
			Android.Net.Uri url = res.DownloadUrl;
			photoUrl = url.ToString();
		}
	}
}