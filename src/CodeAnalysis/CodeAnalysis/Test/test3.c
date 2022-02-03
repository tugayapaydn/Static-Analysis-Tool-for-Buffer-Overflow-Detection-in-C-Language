int main()
{
	char str1[8] = "hello";
	char str2[30] = " world from the mars";
	char str3[6] = "world";

	strcpy(str1, str3);
	strcpy(str1, str2);

	char str4[15] = "hello";
	char str5[7] = " world";

	strcat(str4, str5);
	
	char str6[7] = "hello";
	char str7[30] = " world from the mars";
	
	strcat(str6, str7);
}