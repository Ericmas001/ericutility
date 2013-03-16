﻿using System.Collections.Generic;

// ISO 3166 country code
// Maintenance Agency for ISO 3166 country codes > Lists of country names and code elements > English country names and code elements
// http://www.iso.org/iso/country_codes/iso_3166_code_lists/english_country_names_and_code_elements.htm7

namespace EricUtility2011.Entities
{
    public class CountryInfo
    {
        private static bool m_Loaded = false;
        private static readonly Dictionary<string, string> m_AllCountry = new Dictionary<string, string>();

        public static Dictionary<string, string> AllCountry
        {
            get
            {
                if (!m_Loaded)
                    Load();
                return CountryInfo.m_AllCountry;
            }
        }

        private static void Load()
        {
            if (m_Loaded)
                return;
            m_AllCountry.Clear();
            m_AllCountry.Add("AD", "ANDORRA");
            m_AllCountry.Add("AE", "UNITED ARAB EMIRATES");
            m_AllCountry.Add("AF", "AFGHANISTAN");
            m_AllCountry.Add("AG", "ANTIGUA AND BARBUDA");
            m_AllCountry.Add("AI", "ANGUILLA");
            m_AllCountry.Add("AL", "ALBANIA");
            m_AllCountry.Add("AM", "ARMENIA");
            m_AllCountry.Add("AO", "ANGOLA");
            m_AllCountry.Add("AQ", "ANTARCTICA");
            m_AllCountry.Add("AR", "ARGENTINA");
            m_AllCountry.Add("AS", "AMERICAN SAMOA");
            m_AllCountry.Add("AT", "AUSTRIA");
            m_AllCountry.Add("AU", "AUSTRALIA");
            m_AllCountry.Add("AW", "ARUBA");
            m_AllCountry.Add("AX", "ALAND ISLANDS");
            m_AllCountry.Add("AZ", "AZERBAIJAN");
            m_AllCountry.Add("BA", "BOSNIA AND HERZEGOVINA");
            m_AllCountry.Add("BB", "BARBADOS");
            m_AllCountry.Add("BD", "BANGLADESH");
            m_AllCountry.Add("BE", "BELGIUM");
            m_AllCountry.Add("BF", "BURKINA FASO");
            m_AllCountry.Add("BG", "BULGARIA");
            m_AllCountry.Add("BH", "BAHRAIN");
            m_AllCountry.Add("BI", "BURUNDI");
            m_AllCountry.Add("BJ", "BENIN");
            m_AllCountry.Add("BL", "SAINT BARTHELEMY");
            m_AllCountry.Add("BM", "BERMUDA");
            m_AllCountry.Add("BN", "BRUNEI DARUSSALAM");
            m_AllCountry.Add("BO", "BOLIVIA, PLURINATIONAL STATE OF");
            m_AllCountry.Add("BQ", "BONAIRE, SINT EUSTATIUS AND SABA");
            m_AllCountry.Add("BR", "BRAZIL");
            m_AllCountry.Add("BS", "BAHAMAS");
            m_AllCountry.Add("BT", "BHUTAN");
            m_AllCountry.Add("BV", "BOUVET ISLAND");
            m_AllCountry.Add("BW", "BOTSWANA");
            m_AllCountry.Add("BY", "BELARUS");
            m_AllCountry.Add("BZ", "BELIZE");
            m_AllCountry.Add("CA", "CANADA");
            m_AllCountry.Add("CC", "COCOS (KEELING) ISLANDS");
            m_AllCountry.Add("CD", "CONGO, THE DEMOCRATIC REPUBLIC OF THE");
            m_AllCountry.Add("CF", "CENTRAL AFRICAN REPUBLIC");
            m_AllCountry.Add("CG", "CONGO");
            m_AllCountry.Add("CH", "SWITZERLAND");
            m_AllCountry.Add("CI", "COTE D'IVOIRE");
            m_AllCountry.Add("CK", "COOK ISLANDS");
            m_AllCountry.Add("CL", "CHILE");
            m_AllCountry.Add("CM", "CAMEROON");
            m_AllCountry.Add("CN", "CHINA");
            m_AllCountry.Add("CO", "COLOMBIA");
            m_AllCountry.Add("CR", "COSTA RICA");
            m_AllCountry.Add("CU", "CUBA");
            m_AllCountry.Add("CV", "CAPE VERDE");
            m_AllCountry.Add("CW", "CURACAO");
            m_AllCountry.Add("CX", "CHRISTMAS ISLAND");
            m_AllCountry.Add("CY", "CYPRUS");
            m_AllCountry.Add("CZ", "CZECH REPUBLIC");
            m_AllCountry.Add("DE", "GERMANY");
            m_AllCountry.Add("DJ", "DJIBOUTI");
            m_AllCountry.Add("DK", "DENMARK");
            m_AllCountry.Add("DM", "DOMINICA");
            m_AllCountry.Add("DO", "DOMINICAN REPUBLIC");
            m_AllCountry.Add("DZ", "ALGERIA");
            m_AllCountry.Add("EC", "ECUADOR");
            m_AllCountry.Add("EE", "ESTONIA");
            m_AllCountry.Add("EG", "EGYPT");
            m_AllCountry.Add("EH", "WESTERN SAHARA");
            m_AllCountry.Add("ER", "ERITREA");
            m_AllCountry.Add("ES", "SPAIN");
            m_AllCountry.Add("ET", "ETHIOPIA");
            m_AllCountry.Add("FI", "FINLAND");
            m_AllCountry.Add("FJ", "FIJI");
            m_AllCountry.Add("FK", "FALKLAND ISLANDS (MALVINAS)");
            m_AllCountry.Add("FM", "MICRONESIA, FEDERATED STATES OF");
            m_AllCountry.Add("FO", "FAROE ISLANDS");
            m_AllCountry.Add("FR", "FRANCE");
            m_AllCountry.Add("GA", "GABON");
            m_AllCountry.Add("GB", "UNITED KINGDOM");
            m_AllCountry.Add("GD", "GRENADA");
            m_AllCountry.Add("GE", "GEORGIA");
            m_AllCountry.Add("GF", "FRENCH GUIANA");
            m_AllCountry.Add("GG", "GUERNSEY");
            m_AllCountry.Add("GH", "GHANA");
            m_AllCountry.Add("GI", "GIBRALTAR");
            m_AllCountry.Add("GL", "GREENLAND");
            m_AllCountry.Add("GM", "GAMBIA");
            m_AllCountry.Add("GN", "GUINEA");
            m_AllCountry.Add("GP", "GUADELOUPE");
            m_AllCountry.Add("GQ", "EQUATORIAL GUINEA");
            m_AllCountry.Add("GR", "GREECE");
            m_AllCountry.Add("GS", "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS");
            m_AllCountry.Add("GT", "GUATEMALA");
            m_AllCountry.Add("GU", "GUAM");
            m_AllCountry.Add("GW", "GUINEA-BISSAU");
            m_AllCountry.Add("GY", "GUYANA");
            m_AllCountry.Add("HK", "HONG KONG");
            m_AllCountry.Add("HM", "HEARD ISLAND AND MCDONALD ISLANDS");
            m_AllCountry.Add("HN", "HONDURAS");
            m_AllCountry.Add("HR", "CROATIA");
            m_AllCountry.Add("HT", "HAITI");
            m_AllCountry.Add("HU", "HUNGARY");
            m_AllCountry.Add("ID", "INDONESIA");
            m_AllCountry.Add("IE", "IRELAND");
            m_AllCountry.Add("IL", "ISRAEL");
            m_AllCountry.Add("IM", "ISLE OF MAN");
            m_AllCountry.Add("IN", "INDIA");
            m_AllCountry.Add("IO", "BRITISH INDIAN OCEAN TERRITORY");
            m_AllCountry.Add("IQ", "IRAQ");
            m_AllCountry.Add("IR", "IRAN, ISLAMIC REPUBLIC OF");
            m_AllCountry.Add("IS", "ICELAND");
            m_AllCountry.Add("IT", "ITALY");
            m_AllCountry.Add("JE", "JERSEY");
            m_AllCountry.Add("JM", "JAMAICA");
            m_AllCountry.Add("JO", "JORDAN");
            m_AllCountry.Add("JP", "JAPAN");
            m_AllCountry.Add("KE", "KENYA");
            m_AllCountry.Add("KG", "KYRGYZSTAN");
            m_AllCountry.Add("KH", "CAMBODIA");
            m_AllCountry.Add("KI", "KIRIBATI");
            m_AllCountry.Add("KM", "COMOROS");
            m_AllCountry.Add("KN", "SAINT KITTS AND NEVIS");
            m_AllCountry.Add("KP", "KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF");
            m_AllCountry.Add("KR", "KOREA, REPUBLIC OF");
            m_AllCountry.Add("KW", "KUWAIT");
            m_AllCountry.Add("KY", "CAYMAN ISLANDS");
            m_AllCountry.Add("KZ", "KAZAKHSTAN");
            m_AllCountry.Add("LA", "LAO PEOPLE'S DEMOCRATIC REPUBLIC");
            m_AllCountry.Add("LB", "LEBANON");
            m_AllCountry.Add("LC", "SAINT LUCIA");
            m_AllCountry.Add("LI", "LIECHTENSTEIN");
            m_AllCountry.Add("LK", "SRI LANKA");
            m_AllCountry.Add("LR", "LIBERIA");
            m_AllCountry.Add("LS", "LESOTHO");
            m_AllCountry.Add("LT", "LITHUANIA");
            m_AllCountry.Add("LU", "LUXEMBOURG");
            m_AllCountry.Add("LV", "LATVIA");
            m_AllCountry.Add("LY", "LIBYAN ARAB JAMAHIRIYA");
            m_AllCountry.Add("MA", "MOROCCO");
            m_AllCountry.Add("MC", "MONACO");
            m_AllCountry.Add("MD", "MOLDOVA, REPUBLIC OF");
            m_AllCountry.Add("ME", "MONTENEGRO");
            m_AllCountry.Add("MF", "SAINT MARTIN (FRENCH PART)");
            m_AllCountry.Add("MG", "MADAGASCAR");
            m_AllCountry.Add("MH", "MARSHALL ISLANDS");
            m_AllCountry.Add("MK", "MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF");
            m_AllCountry.Add("ML", "MALI");
            m_AllCountry.Add("MM", "MYANMAR");
            m_AllCountry.Add("MN", "MONGOLIA");
            m_AllCountry.Add("MO", "MACAO");
            m_AllCountry.Add("MP", "NORTHERN MARIANA ISLANDS");
            m_AllCountry.Add("MQ", "MARTINIQUE");
            m_AllCountry.Add("MR", "MAURITANIA");
            m_AllCountry.Add("MS", "MONTSERRAT");
            m_AllCountry.Add("MT", "MALTA");
            m_AllCountry.Add("MU", "MAURITIUS");
            m_AllCountry.Add("MV", "MALDIVES");
            m_AllCountry.Add("MW", "MALAWI");
            m_AllCountry.Add("MX", "MEXICO");
            m_AllCountry.Add("MY", "MALAYSIA");
            m_AllCountry.Add("MZ", "MOZAMBIQUE");
            m_AllCountry.Add("NA", "NAMIBIA");
            m_AllCountry.Add("NC", "NEW CALEDONIA");
            m_AllCountry.Add("NE", "NIGER");
            m_AllCountry.Add("NF", "NORFOLK ISLAND");
            m_AllCountry.Add("NG", "NIGERIA");
            m_AllCountry.Add("NI", "NICARAGUA");
            m_AllCountry.Add("NL", "NETHERLANDS");
            m_AllCountry.Add("NO", "NORWAY");
            m_AllCountry.Add("NP", "NEPAL");
            m_AllCountry.Add("NR", "NAURU");
            m_AllCountry.Add("NU", "NIUE");
            m_AllCountry.Add("NZ", "NEW ZEALAND");
            m_AllCountry.Add("OM", "OMAN");
            m_AllCountry.Add("PA", "PANAMA");
            m_AllCountry.Add("PE", "PERU");
            m_AllCountry.Add("PF", "FRENCH POLYNESIA");
            m_AllCountry.Add("PG", "PAPUA NEW GUINEA");
            m_AllCountry.Add("PH", "PHILIPPINES");
            m_AllCountry.Add("PK", "PAKISTAN");
            m_AllCountry.Add("PL", "POLAND");
            m_AllCountry.Add("PM", "SAINT PIERRE AND MIQUELON");
            m_AllCountry.Add("PN", "PITCAIRN");
            m_AllCountry.Add("PR", "PUERTO RICO");
            m_AllCountry.Add("PS", "PALESTINIAN TERRITORY, OCCUPIED");
            m_AllCountry.Add("PT", "PORTUGAL");
            m_AllCountry.Add("PW", "PALAU");
            m_AllCountry.Add("PY", "PARAGUAY");
            m_AllCountry.Add("QA", "QATAR");
            m_AllCountry.Add("RE", "REUNION");
            m_AllCountry.Add("RO", "ROMANIA");
            m_AllCountry.Add("RS", "SERBIA");
            m_AllCountry.Add("RU", "RUSSIAN FEDERATION");
            m_AllCountry.Add("RW", "RWANDA");
            m_AllCountry.Add("SA", "SAUDI ARABIA");
            m_AllCountry.Add("SB", "SOLOMON ISLANDS");
            m_AllCountry.Add("SC", "SEYCHELLES");
            m_AllCountry.Add("SD", "SUDAN");
            m_AllCountry.Add("SE", "SWEDEN");
            m_AllCountry.Add("SG", "SINGAPORE");
            m_AllCountry.Add("SH", "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA");
            m_AllCountry.Add("SI", "SLOVENIA");
            m_AllCountry.Add("SJ", "SVALBARD AND JAN MAYEN");
            m_AllCountry.Add("SK", "SLOVAKIA");
            m_AllCountry.Add("SL", "SIERRA LEONE");
            m_AllCountry.Add("SM", "SAN MARINO");
            m_AllCountry.Add("SN", "SENEGAL");
            m_AllCountry.Add("SO", "SOMALIA");
            m_AllCountry.Add("SR", "SURINAME");
            m_AllCountry.Add("ST", "SAO TOME AND PRINCIPE");
            m_AllCountry.Add("SV", "EL SALVADOR");
            m_AllCountry.Add("SX", "SINT MAARTEN (DUTCH PART)");
            m_AllCountry.Add("SY", "SYRIAN ARAB REPUBLIC");
            m_AllCountry.Add("SZ", "SWAZILAND");
            m_AllCountry.Add("TC", "TURKS AND CAICOS ISLANDS");
            m_AllCountry.Add("TD", "CHAD");
            m_AllCountry.Add("TF", "FRENCH SOUTHERN TERRITORIES");
            m_AllCountry.Add("TG", "TOGO");
            m_AllCountry.Add("TH", "THAILAND");
            m_AllCountry.Add("TJ", "TAJIKISTAN");
            m_AllCountry.Add("TK", "TOKELAU");
            m_AllCountry.Add("TL", "TIMOR-LESTE");
            m_AllCountry.Add("TM", "TURKMENISTAN");
            m_AllCountry.Add("TN", "TUNISIA");
            m_AllCountry.Add("TO", "TONGA");
            m_AllCountry.Add("TR", "TURKEY");
            m_AllCountry.Add("TT", "TRINIDAD AND TOBAGO");
            m_AllCountry.Add("TV", "TUVALU");
            m_AllCountry.Add("TW", "TAIWAN, PROVINCE OF CHINA");
            m_AllCountry.Add("TZ", "TANZANIA, UNITED REPUBLIC OF");
            m_AllCountry.Add("UA", "UKRAINE");
            m_AllCountry.Add("UG", "UGANDA");
            m_AllCountry.Add("UM", "UNITED STATES MINOR OUTLYING ISLANDS");
            m_AllCountry.Add("US", "UNITED STATES");
            m_AllCountry.Add("UY", "URUGUAY");
            m_AllCountry.Add("UZ", "UZBEKISTAN");
            m_AllCountry.Add("VA", "HOLY SEE (VATICAN CITY STATE)");
            m_AllCountry.Add("VC", "SAINT VINCENT AND THE GRENADINES");
            m_AllCountry.Add("VE", "VENEZUELA, BOLIVARIAN REPUBLIC OF");
            m_AllCountry.Add("VG", "VIRGIN ISLANDS, BRITISH");
            m_AllCountry.Add("VI", "VIRGIN ISLANDS, U.S.");
            m_AllCountry.Add("VN", "VIET NAM");
            m_AllCountry.Add("VU", "VANUATU");
            m_AllCountry.Add("WF", "WALLIS AND FUTUNA");
            m_AllCountry.Add("WS", "SAMOA");
            m_AllCountry.Add("YE", "YEMEN");
            m_AllCountry.Add("YT", "MAYOTTE");
            m_AllCountry.Add("ZA", "SOUTH AFRICA");
            m_AllCountry.Add("ZM", "ZAMBIA");
            m_AllCountry.Add("ZW", "ZIMBABWE");
            m_Loaded = true;
        }
    }
}