int main()
{
	int arr[4] = { 5, 8, 3, 124 };

	int i = 0;
	int b = -2147483645;

	if (i >= 0)
	{
		while (i < 6)
		{
			arr[i] = 0;
			b = b - 1;

			i = i + 1;
		}
	}

	arr[3] = 6;
	arr[4] = 0;
}
