using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
	public static void Move(this IList list, int iIndexToMove, int direction)
	{
		//up
		if (direction == 1 && iIndexToMove >0)
		{
		
			var old = list[iIndexToMove - 1];
			list[iIndexToMove -1] = list[iIndexToMove];
			list[iIndexToMove] = old;
		}
		else if(direction != 1 && iIndexToMove<list.Count-1)
		{
			var old = list[iIndexToMove + 1];
			list[iIndexToMove + 1] = list[iIndexToMove];
			list[iIndexToMove] = old;
		}
	}

	public static void Swap(this IList list,int firstIndex,int secondIndex) {
		if (list != null && firstIndex >= 0 &&
		    firstIndex < list.Count && secondIndex >= 0 &&
		    secondIndex < list.Count) {

			if (firstIndex == secondIndex) {
				return;
			}
			var temp = list [firstIndex];
			list [firstIndex] = list [secondIndex];
			list [secondIndex] = temp;
		}
	}
}
