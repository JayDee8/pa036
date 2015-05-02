CREATE TABLE "uzivatel" (
  "uzivatel_id" int NOT NULL,
  "jmeno" varchar(40) NOT NULL,
  "prijmeni" varchar(160) NOT NULL,
  "adresa_id" int NOT NULL,
  "poznamka" varchar(500),
  "penezenka" int,

    CONSTRAINT PK_uzivatel_uzivatel_id PRIMARY KEY CLUSTERED (uzivatel_id)
);

CREATE TABLE "adresa" (
  "adresa_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "mesto" varchar(50) NOT NULL,
  "ulice" varchar(50) NOT NULL,
  "PSC" varchar(5) NOT NULL,
  "telefon" varchar(15) NOT NULL,
  "email" varchar(50) NOT NULL,

  CONSTRAINT PK_adresa_adresa_id PRIMARY KEY CLUSTERED (adresa_id)
);

CREATE TABLE "sledovane" (
  "sledovane_id" int NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "uzivatel_id" int NOT NULL UNIQUE,
  "akcie_id" int NOT NULL UNIQUE,

  CONSTRAINT PK_sledovane_sledovane_id PRIMARY KEY CLUSTERED (sledovane_id)
);

CREATE TABLE "pokl_poukazky" (
  "poukazka_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "zkratka" varchar(10) NOT NULL,

  CONSTRAINT PK_pokl_poukazky_poukazka_id PRIMARY KEY CLUSTERED (poukazka_id)
);

CREATE TABLE "historie_poukazek" (
  "poukazka_id" int NOT NULL UNIQUE,
  "datum" datetime NOT NULL,
  "urok" float(10) NOT NULL,

  CONSTRAINT PK_historie_poukazek_poukazka_id PRIMARY KEY CLUSTERED (poukazka_id)
);

CREATE TABLE "portfolio" (
  "uzivatel_id" int NOT NULL UNIQUE,
  "akcie_id" int NOT NULL UNIQUE,
  "pocet" int,
  "datum_pridani" datetime NOT NULL,
  "datum_odebrani" datetime,

  CONSTRAINT PK_portfolio_uzivatel_id_akcie_id PRIMARY KEY CLUSTERED (uzivatel_id,akcie_id)
);

CREATE TABLE "akcie" (
  "akcie_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "firma_id" int NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "zkratka" varchar(20) NOT NULL,

  CONSTRAINT PK_akcie_akcie_id PRIMARY KEY CLUSTERED (akcie_id)
);

CREATE TABLE "historie_akcie" (
  "akcie_id" int NOT NULL UNIQUE,
  "datum" datetime NOT NULL,
  "cena_prodej" float(10) NOT NULL,
  "cena_nakup" float(10) NOT NULL,

  CONSTRAINT PK_historie_akcie_akcie_id_datum PRIMARY KEY CLUSTERED (akcie_id,datum)
);

CREATE TABLE "firma" (
  "firma_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "adresa_id" int NOT NULL UNIQUE,
  "nazev" float(10) NOT NULL,
  "ICO" varchar(8) NOT NULL,
  "DIC" varchar(10) NOT NULL,

  CONSTRAINT PK_firma_firma_id PRIMARY KEY CLUSTERED (firma_id)
);

CREATE TABLE "index_PX" (
  "datum" datetime NOT NULL UNIQUE,
  "hodnota" float(10) NOT NULL,

  CONSTRAINT PK_index_PX_datum PRIMARY KEY CLUSTERED (datum)
);
