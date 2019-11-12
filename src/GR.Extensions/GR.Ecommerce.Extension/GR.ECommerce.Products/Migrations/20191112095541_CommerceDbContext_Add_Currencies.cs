﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.ECommerce.BaseImplementations.Migrations
{
    public partial class CommerceDbContext_Add_Currencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "Commerce",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Symbol = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PluralName = table.Column<string>(nullable: true),
                    NativeSymbol = table.Column<string>(nullable: false),
                    DecimalDigits = table.Column<int>(nullable: false),
                    Rounding = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.InsertData(
                schema: "Commerce",
                table: "Currencies",
                columns: new[] { "Code", "DecimalDigits", "Name", "NativeSymbol", "PluralName", "Rounding", "Symbol" },
                values: new object[,]
                {
                    { "USD", 0, "US Dollar", "$", null, 0m, "$" },
                    { "PHP", 0, "Philippine Peso", "₱", "Philippine pesos", 0m, "₱" },
                    { "PEN", 0, "Peruvian Nuevo Sol", "S/.", "Peruvian nuevos soles", 0m, "S/." },
                    { "PAB", 0, "Panamanian Balboa", "B/.", "Panamanian balboas", 0m, "B/." },
                    { "OMR", 0, "Omani Rial", "ر.ع.‏", "Omani rials", 0m, "OMR" },
                    { "NZD", 0, "New Zealand Dollar", "$", "New Zealand dollars", 0m, "NZ$" },
                    { "NPR", 0, "Nepalese Rupee", "नेरू", "Nepalese rupees", 0m, "NPRs" },
                    { "NOK", 0, "Norwegian Krone", "kr", "Norwegian kroner", 0m, "Nkr" },
                    { "NIO", 0, "Nicaraguan Córdoba", "C$", "Nicaraguan córdobas", 0m, "C$" },
                    { "NGN", 0, "Nigerian Naira", "₦", "Nigerian nairas", 0m, "₦" },
                    { "NAD", 0, "Namibian Dollar", "N$", "Namibian dollars", 0m, "N$" },
                    { "MZN", 0, "Mozambican Metical", "MTn", "Mozambican meticals", 0m, "MTn" },
                    { "MYR", 0, "Malaysian Ringgit", "RM", "Malaysian ringgits", 0m, "RM" },
                    { "MXN", 0, "Mexican Peso", "$", "Mexican pesos", 0m, "MX$" },
                    { "MUR", 0, "Mauritian Rupee", "MURs", "Mauritian rupees", 0m, "MURs" },
                    { "MOP", 0, "Macanese Pataca", "MOP$", "Macanese patacas", 0m, "MOP$" },
                    { "MMK", 0, "Myanma Kyat", "K", "Myanma kyats", 0m, "MMK" },
                    { "MKD", 0, "Macedonian Denar", "MKD", "Macedonian denari", 0m, "MKD" },
                    { "MGA", 0, "Malagasy Ariary", "MGA", "Malagasy Ariaries", 0m, "MGA" },
                    { "MDL", 0, "Moldovan Leu", "MDL", "Moldovan lei", 0m, "MDL" },
                    { "MAD", 0, "Moroccan Dirham", "د.م.‏", "Moroccan dirhams", 0m, "MAD" },
                    { "LYD", 0, "Libyan Dinar", "د.ل.‏", "Libyan dinars", 0m, "LD" },
                    { "LVL", 0, "Latvian Lats", "Ls", "Latvian lati", 0m, "Ls" },
                    { "LTL", 0, "Lithuanian Litas", "Lt", "Lithuanian litai", 0m, "Lt" },
                    { "LKR", 0, "Sri Lankan Rupee", "SL Re", "Sri Lankan rupees", 0m, "SLRs" },
                    { "LBP", 0, "Lebanese Pound", "ل.ل.‏", "Lebanese pounds", 0m, "LB£" },
                    { "PKR", 0, "Pakistani Rupee", "₨", "Pakistani rupees", 0m, "PKRs" },
                    { "PLN", 0, "Polish Zloty", "zł", "Polish zlotys", 0m, "zł" },
                    { "PYG", 0, "Paraguayan Guarani", "₲", "Paraguayan guaranis", 0m, "₲" },
                    { "QAR", 0, "Qatari Rial", "ر.ق.‏", "Qatari rials", 0m, "QR" },
                    { "YER", 0, "Yemeni Rial", "ر.ي.‏", "Yemeni rials", 0m, "YR" },
                    { "XOF", 0, "CFA Franc BCEAO", "CFA", "CFA francs BCEAO", 0m, "CFA" },
                    { "XAF", 0, "CFA Franc BEAC", "FCFA", "CFA francs BEAC", 0m, "FCFA" },
                    { "VND", 0, "Vietnamese Dong", "₫", "Vietnamese dong", 0m, "₫" },
                    { "VEF", 0, "Venezuelan Bolívar", "Bs.F.", "Venezuelan bolívars", 0m, "Bs.F." },
                    { "UZS", 0, "Uzbekistan Som", "UZS", "Uzbekistan som", 0m, "UZS" },
                    { "UYU", 0, "Uruguayan Peso", "$", "Uruguayan pesos", 0m, "$U" },
                    { "UGX", 0, "Ugandan Shilling", "USh", "Ugandan shillings", 0m, "USh" },
                    { "UAH", 0, "Ukrainian Hryvnia", "₴", "Ukrainian hryvnias", 0m, "₴" },
                    { "TZS", 0, "Tanzanian Shilling", "TSh", "Tanzanian shillings", 0m, "TSh" },
                    { "TWD", 0, "New Taiwan Dollar", "NT$", "New Taiwan dollars", 0m, "NT$" },
                    { "TTD", 0, "Trinidad and Tobago Dollar", "$", "Trinidad and Tobago dollars", 0m, "TT$" },
                    { "KZT", 0, "Kazakhstani Tenge", "тңг.", "Kazakhstani tenges", 0m, "KZT" },
                    { "TRY", 0, "Turkish Lira", "TL", "Turkish Lira", 0m, "TL" },
                    { "TND", 0, "Tunisian Dinar", "د.ت.‏", "Tunisian dinars", 0m, "DT" },
                    { "THB", 0, "Thai Baht", "฿", "Thai baht", 0m, "฿" },
                    { "SYP", 0, "Syrian Pound", "ل.س.‏", "Syrian pounds", 0m, "SY£" },
                    { "SOS", 0, "Somali Shilling", "Ssh", "Somali shillings", 0m, "Ssh" },
                    { "SGD", 0, "Singapore Dollar", "$", "Singapore dollars", 0m, "S$" },
                    { "SEK", 0, "Swedish Krona", "kr", "Swedish kronor", 0m, "Skr" },
                    { "SDG", 0, "Sudanese Pound", "SDG", "Sudanese pounds", 0m, "SDG" },
                    { "SAR", 0, "Saudi Riyal", "ر.س.‏", "Saudi riyals", 0m, "SR" },
                    { "RWF", 0, "Rwandan Franc", "FR", "Rwandan francs", 0m, "RWF" },
                    { "RUB", 0, "Russian Ruble", "руб.", "Russian rubles", 0m, "RUB" },
                    { "RSD", 0, "Serbian Dinar", "дин.", "Serbian dinars", 0m, "din." },
                    { "RON", 0, "Romanian Leu", "RON", "Romanian lei", 0m, "RON" },
                    { "TOP", 0, "Tongan Paʻanga", "T$", "Tongan paʻanga", 0m, "T$" },
                    { "KWD", 0, "Kuwaiti Dinar", "د.ك.‏", "Kuwaiti dinars", 0m, "KD" },
                    { "KRW", 0, "South Korean Won", "₩", "South Korean won", 0m, "₩" },
                    { "KMF", 0, "Comorian Franc", "FC", "Comorian francs", 0m, "CF" },
                    { "CRC", 0, "Costa Rican Colón", "₡", "Costa Rican colóns", 0m, "₡" },
                    { "COP", 0, "Colombian Peso", "$", "Colombian pesos", 0m, "CO$" },
                    { "CNY", 0, "Chinese Yuan", "CN¥", "Chinese yuan", 0m, "CN¥" },
                    { "CLP", 0, "Chilean Peso", "$", "Chilean pesos", 0m, "CL$" },
                    { "CHF", 0, "Swiss Franc", "CHF", "Swiss francs", 0.05m, "CHF" },
                    { "CDF", 0, "Congolese Franc", "FrCD", "Congolese francs", 0m, "CDF" },
                    { "BZD", 0, "Belize Dollar", "$", "Belize dollars", 0m, "BZ$" },
                    { "BYR", 0, "Belarusian Ruble", "BYR", "Belarusian rubles", 0m, "BYR" },
                    { "BWP", 0, "Botswanan Pula", "P", "Botswanan pulas", 0m, "BWP" },
                    { "BRL", 0, "Brazilian Real", "R$", "Brazilian reals", 0m, "R$" },
                    { "BOB", 0, "Bolivian Boliviano", "Bs", "Bolivian bolivianos", 0m, "Bs" },
                    { "BND", 0, "Brunei Dollar", "$", "Brunei dollars", 0m, "BN$" },
                    { "CVE", 0, "Cape Verdean Escudo", "CV$", "Cape Verdean escudos", 0m, "CV$" },
                    { "BIF", 0, "Burundian Franc", "FBu", "Burundian francs", 0m, "FBu" },
                    { "BGN", 0, "Bulgarian Lev", "лв.", "Bulgarian leva", 0m, "BGN" },
                    { "BDT", 0, "Bangladeshi Taka", "৳", "Bangladeshi takas", 0m, "Tk" },
                    { "BAM", 0, "Bosnia-Herzegovina Convertible Mark", "KM", "Bosnia-Herzegovina convertible marks", 0m, "KM" },
                    { "AZN", 0, "Azerbaijani Manat", "ман.", "Azerbaijani manats", 0m, "man." },
                    { "AUD", 0, "Australian Dollar", "$", "Australian dollars", 0m, "AU$" },
                    { "ARS", 0, "Argentine Peso", "$", "Argentine pesos", 0m, "AR$" },
                    { "AMD", 0, "Armenian Dram", "դր.", "Armenian drams", 0m, "AMD" },
                    { "ALL", 0, "Albanian Lek", "Lek", "Albanian lekë", 0m, "ALL" },
                    { "AFN", 0, "Afghan Afghani", "؋", "Afghan Afghanis", 0m, "Af" },
                    { "AED", 0, "United Arab Emirates Dirham", "د.إ.‏", "UAE dirhams", 0m, "AED" },
                    { "EUR", 0, "Euro", "€", "euros", 0m, "€" },
                    { "CAD", 0, "Canadian Dollar", "$", "Canadian dollars", 0m, "CA$" },
                    { "BHD", 0, "Bahraini Dinar", "د.ب.‏", "Bahraini dinars", 0m, "BD" },
                    { "ZAR", 0, "South African Rand", "R", "South African rand", 0m, "R" },
                    { "CZK", 0, "Czech Republic Koruna", "Kč", "Czech Republic korunas", 0m, "Kč" },
                    { "DKK", 0, "Danish Krone", "kr", "Danish kroner", 0m, "Dkr" },
                    { "KHR", 0, "Cambodian Riel", "៛", "Cambodian riels", 0m, "KHR" },
                    { "KES", 0, "Kenyan Shilling", "Ksh", "Kenyan shillings", 0m, "Ksh" },
                    { "JPY", 0, "Japanese Yen", "￥", "Japanese yen", 0m, "¥" },
                    { "JOD", 0, "Jordanian Dinar", "د.أ.‏", "Jordanian dinars", 0m, "JD" },
                    { "JMD", 0, "Jamaican Dollar", "$", "Jamaican dollars", 0m, "J$" },
                    { "ISK", 0, "Icelandic Króna", "kr", "Icelandic krónur", 0m, "Ikr" },
                    { "IRR", 0, "Iranian Rial", "﷼", "Iranian rials", 0m, "IRR" },
                    { "IQD", 0, "Iraqi Dinar", "د.ع.‏", "Iraqi dinars", 0m, "IQD" },
                    { "INR", 0, "Indian Rupee", "টকা", "Indian rupees", 0m, "Rs" },
                    { "ILS", 0, "Israeli New Sheqel", "₪", "Israeli new sheqels", 0m, "₪" },
                    { "IDR", 0, "Indonesian Rupiah", "Rp", "Indonesian rupiahs", 0m, "Rp" },
                    { "HUF", 0, "Hungarian Forint", "Ft", "Hungarian forints", 0m, "Ft" },
                    { "DJF", 0, "Djiboutian Franc", "Fdj", "Djiboutian francs", 0m, "Fdj" },
                    { "HRK", 0, "Croatian Kuna", "kn", "Croatian kunas", 0m, "kn" },
                    { "HKD", 0, "Hong Kong Dollar", "$", "Hong Kong dollars", 0m, "HK$" },
                    { "GTQ", 0, "Guatemalan Quetzal", "Q", "Guatemalan quetzals", 0m, "GTQ" },
                    { "GNF", 0, "Guinean Franc", "FG", "Guinean francs", 0m, "FG" },
                    { "GHS", 0, "Ghanaian Cedi", "GH₵", "Ghanaian cedis", 0m, "GH₵" },
                    { "GEL", 0, "Georgian Lari", "GEL", "Georgian laris", 0m, "GEL" },
                    { "GBP", 0, "British Pound Sterling", "£", "British pounds sterling", 0m, "£" },
                    { "ETB", 0, "Ethiopian Birr", "Br", "Ethiopian birrs", 0m, "Br" },
                    { "ERN", 0, "Eritrean Nakfa", "Nfk", "Eritrean nakfas", 0m, "Nfk" },
                    { "EGP", 0, "Egyptian Pound", "ج.م.‏", "Egyptian pounds", 0m, "EGP" },
                    { "EEK", 0, "Estonian Kroon", "kr", "Estonian kroons", 0m, "Ekr" },
                    { "DZD", 0, "Algerian Dinar", "د.ج.‏", "Algerian dinars", 0m, "DA" },
                    { "DOP", 0, "Dominican Peso", "RD$", "Dominican pesos", 0m, "RD$" },
                    { "HNL", 0, "Honduran Lempira", "L", "Honduran lempiras", 0m, "HNL" },
                    { "ZMK", 0, "Zambian Kwacha", "ZK", "Zambian kwachas", 0m, "ZK" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "Commerce");
        }
    }
}
