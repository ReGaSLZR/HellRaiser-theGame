namespace GamePlay.Item
{

	using GamePlay.Base;
	using UnityEngine;
	using NaughtyAttributes;
	using Utils;

	public class StashItemsTrigger : BaseTrigger
	{

		[SerializeField]
        [Tooltip("Required: All items must be children of this gameObject " +
			"to achieve an Object-Pooling-ish style.")]
		private StashItem[] m_items;

		private void Awake()
		{
			if (m_items.Length == 0)
			{
				LogUtil.PrintInfo(this, GetType(), "What is a stash without items in it? Destroying. XD");
				Destroy(gameObject);
            }
        }

		protected override void Start()
		{
			//This class purposely bypasses checking for Player-tag collision.
			//base.Start(); 

			DeactivateAllItems();
        }

		private void DeactivateAllItems()
		{
			for (int x=0; x<m_items.Length; x++)
			{
				m_items[x].m_childStashItem.SetActive(false);
            }
        }

		private bool ShouldActivate(StashItem item)
		{
			return (item.m_activateChance >= Random.Range(0, StashItem.MAX_CHANCE));
        }

		public override void Execute()
		{
			for (int x=0; x<m_items.Length; x++)
			{
				StashItem item = m_items[x];
				if (ShouldActivate(item))
				{
					item.m_childStashItem.SetActive(true);
					item.m_childStashItem.transform.SetParent(null);
                }
            }

			Destroy(this);
        }

	}

    [System.Serializable]
	public class StashItem
	{

		public const int MAX_CHANCE = 100;

		[Range(0, MAX_CHANCE)]
		public int m_activateChance = 50;

        [Required]
		public GameObject m_childStashItem;

    }

}