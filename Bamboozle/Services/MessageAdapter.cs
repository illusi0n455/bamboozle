using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using static Android.App.ActionBar;
using Com.Bumptech.Glide;
using Firebase.Storage;

namespace Bamboozle
{
	public class MessageAdapter :  ArrayAdapter<MessageContent>
	{
		private List<MessageContent> _messageList;
		private Activity _context;
		public MessageAdapter(Activity context, List<MessageContent> messageList)
			: base(context, Resource.Layout.ChatBubble, messageList)
		{
			this._context = context;
			this._messageList = messageList;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = this._messageList[position];
			var view = (convertView ??
				   this._context.LayoutInflater.Inflate(
				   Resource.Layout.ChatBubble,
				   parent,
				   false)) as LinearLayout;

			TextView username = view.FindViewById<TextView>(Resource.Id.txtBubbleFrom);
			TextView message = view.FindViewById<TextView>(Resource.Id.txtBubbleText);
			RelativeLayout messageBubble = view.FindViewById<RelativeLayout>(Resource.Id.lytBubble);
			ImageView imgPhoto = view.FindViewById<ImageView>(Resource.Id.imgPhoto);
			//Glide.With(_context).Load("").Into(imgPhoto);
			if (item.Photo != "none")
			{
				//StorageReference storageReference = FirebaseStorage.Instance.GetReference(item.Photo);
				Glide.With(_context).Load(item.Photo).Into(imgPhoto);
				imgPhoto.Visibility = ViewStates.Visible;
			}
			else
			{
				imgPhoto.Visibility = ViewStates.Gone;
			}
			username.Text = item.From;
			message.Text = item.Text;
			if (item.From == FirebaseAuth.Instance.CurrentUser.Email)
			{
				view.SetGravity(GravityFlags.Right);
				messageBubble.SetBackgroundResource(Resource.Drawable.bubble_green);
			}
			else
			{
				view.SetGravity(GravityFlags.Left);
				messageBubble.SetBackgroundResource(Resource.Drawable.bubble_yellow);
			}

			return view;
		}
	}
}