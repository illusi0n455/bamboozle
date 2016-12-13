using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Firebase.Auth;

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
			username.Text = item.From;
			message.Text = item.Text;
			if (item.From == FirebaseAuth.Instance.CurrentUser.Email)
			{
				view.SetGravity(GravityFlags.Left);
				message.SetBackgroundResource(Resource.Drawable.bubble_yellow);
			}
			else
			{
				view.SetGravity(GravityFlags.Right);
				message.SetBackgroundResource(Resource.Drawable.bubble_green);
			}

			return view;
		}
	}
}