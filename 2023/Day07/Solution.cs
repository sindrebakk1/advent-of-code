using System;
using AdventOfCode.Common;

namespace AdventOfCode.Y2023.Day7;

[ProblemName("Camel Cards")]
public partial class Solution : Solver
{
	private static Dictionary<char, int> cardMap = [];
	public object PartOne(string input)
	{
		cardMap = new()
		{
			{ 'A', 14 },
			{ 'K', 13 },
			{ 'Q', 12 },
			{ 'J', 11 },
			{ 'T', 10 },
			{ '9', 9 },
			{ '8', 8 },
			{ '7', 7 },
			{ '6', 6 },
			{ '5', 5 },
			{ '4', 4 },
			{ '3', 3 },
			{ '2', 2 },
		};
		var hands = input.Split("\n").Select(line => {
			var handString = line.Split(' ');
			return new Hand(
				handString[0],
				GetHandType(handString[0]),
				int.Parse(handString[1])
			);
		}).ToList();

		List<Hand> playedHands = [];
		foreach (var hand in hands)
		{
			RankHand(hand, playedHands);
		}

		int sum = 0;
		for (int i = 0; i < playedHands.Count; i++)
		{
			sum += (i + 1) * playedHands[i].Bid;
		}

		return sum;
	}
	public object PartTwo(string input)
	{
		cardMap = new()
		{
			{ 'A', 13 },
			{ 'K', 12 },
			{ 'Q', 11 },
			{ 'T', 10 },
			{ '9', 9 },
			{ '8', 8 },
			{ '7', 7 },
			{ '6', 6 },
			{ '5', 5 },
			{ '4', 4 },
			{ '3', 3 },
			{ '2', 2 },
			{ 'J', 1 },
		};
		
		var hands = input.Split("\n").Select(line => {
			var handString = line.Split(' ');
			return new Hand(
				handString[0],
				GetHandType(handString[0], true),
				int.Parse(handString[1])
			);
		}).ToList();

		List<Hand> playedHands = [];
		foreach (var hand in hands)
		{
			RankHand(hand, playedHands);
		}

		int sum = 0;
		for (int i = 0; i < playedHands.Count; i++)
		{
			sum += (i + 1) * playedHands[i].Bid;
		}

		return sum;
	}

	private static HandType GetHandType(string handString, bool joker = false)
	{
		var duplicates = GetDuplicates(handString, joker);

		if (joker)
		{
			if (duplicates.TryGetValue('J', out int jokers))
			{
				duplicates.Remove('J');
				if (!duplicates.Any())
				{
					return HandType.FiveOfAKind;
				}
				duplicates[duplicates.MaxBy(entry => entry.Value).Key] += jokers;
			}
		}


		int fiveOfAKind = 0;
		int fourOfAKind = 0;
		int threeOfAKind = 0;
		int pairs = 0;


		foreach (var item in duplicates.AsEnumerable())
		{
			switch (item.Value)
			{
				case 5:
					fiveOfAKind++;
					break;
				case 4:
					fourOfAKind++;
					break;
				case 3:
					threeOfAKind++;
					break;
				case 2:
					pairs++;
					break;
				default:
					break;
			}
			if (fiveOfAKind > 0) {
				return HandType.FiveOfAKind;
			}
			if (fourOfAKind > 0) {
				return HandType.FourOfAKind;
			}
		}

		if (threeOfAKind > 0 && pairs > 0) {
			return HandType.FullHouse;
		}
		if (threeOfAKind > 0) {
			return HandType.ThreeOfAKind;
		}
		if (pairs >= 2) {
			return HandType.TwoPair;
		}
		if (pairs == 1) {
			return HandType.OnePair;
		}
		return HandType.HighCard;
	}

	private static Dictionary<char, int> GetDuplicates(string handString, bool joker)
	{
		Dictionary<char, int> duplicates = [];

		foreach (char cardChar in handString)
		{
			duplicates[cardChar] = (duplicates.TryGetValue(cardChar, out int value) ? value : 0) + 1;
		}

		return duplicates;
	}

	private static void RankHand(Hand currentHand, List<Hand> playedHands)
	{
		if (!playedHands.Any()) {
			playedHands.Add(currentHand);
			return;
		}

		if (currentHand.Type > playedHands.Last().Type) {
			playedHands.Add(currentHand);
			return;
		}
		
		if (currentHand.Type < playedHands.First().Type) {
			playedHands.Insert(0, currentHand);
			return;
		}

		for (int i = 0; i < playedHands.Count; i++)
		{
			if (IsBetter(playedHands[i], currentHand)) {
				playedHands.Insert(i, currentHand);
				return;
			}
		}
		playedHands.Add(currentHand);
	}

	private static bool IsBetter(Hand currentHand, Hand handToCompare)
	{
		if (currentHand.Type < handToCompare.Type) {
			return false;
		}
		if (currentHand.Type > handToCompare.Type) {
			return true;
		}

		string currentHandString = currentHand.HandString;
		string handToCompareString = handToCompare.HandString;
		for (int i = 0; i < currentHandString.Length; i++) {
			if (cardMap[currentHandString[i]] < cardMap[handToCompareString[i]]) {
				return false;
			}
			if (cardMap[currentHandString[i]] > cardMap[handToCompareString[i]]) {
				return true;
			}
		}
		return false;
	}
}

enum HandType
{
	HighCard,
	OnePair,
	TwoPair,
	ThreeOfAKind,
	FullHouse,
	FourOfAKind,
	FiveOfAKind,
}

record Hand(string HandString, HandType Type, int Bid);
