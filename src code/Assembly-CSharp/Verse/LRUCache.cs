using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000028 RID: 40
	public class LRUCache<K, V>
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00009429 File Offset: 0x00007629
		public LRUCache(int capacity)
		{
			this.capacity = capacity;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009450 File Offset: 0x00007650
		public bool TryGetValue(K key, out V result)
		{
			LinkedListNode<ValueTuple<K, V>> linkedListNode;
			if (this.cache.TryGetValue(key, out linkedListNode))
			{
				result = linkedListNode.Value.Item2;
				this.WasUsed(linkedListNode);
				return true;
			}
			result = default(V);
			return false;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009490 File Offset: 0x00007690
		public void Add(K key, V value)
		{
			if (this.cache.Count > this.capacity)
			{
				this.RemoveLeastUsed();
			}
			LinkedListNode<ValueTuple<K, V>> linkedListNode = new LinkedListNode<ValueTuple<K, V>>(new ValueTuple<K, V>(key, value));
			this.cache.Add(key, linkedListNode);
			this.leastRecentList.AddLast(linkedListNode);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000094DC File Offset: 0x000076DC
		public void Clear()
		{
			this.cache.Clear();
			this.leastRecentList.Clear();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000094F4 File Offset: 0x000076F4
		private void WasUsed(LinkedListNode<ValueTuple<K, V>> node)
		{
			this.leastRecentList.Remove(node);
			this.leastRecentList.AddLast(node);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009510 File Offset: 0x00007710
		private void RemoveLeastUsed()
		{
			LinkedListNode<ValueTuple<K, V>> first = this.leastRecentList.First;
			if (first != null)
			{
				this.leastRecentList.RemoveFirst();
				this.cache.Remove(first.Value.Item1);
			}
		}

		// Token: 0x0400006B RID: 107
		private readonly Dictionary<K, LinkedListNode<ValueTuple<K, V>>> cache = new Dictionary<K, LinkedListNode<ValueTuple<K, V>>>();

		// Token: 0x0400006C RID: 108
		private readonly LinkedList<ValueTuple<K, V>> leastRecentList = new LinkedList<ValueTuple<K, V>>();

		// Token: 0x0400006D RID: 109
		private readonly int capacity;
	}
}
