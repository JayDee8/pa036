CREATE TABLE "uzivatel" (
  "uzivatel_id" int NOT NULL,
  "jmeno" varchar(40) NOT NULL,
  "prijmeni" varchar(160) NOT NULL,
  "adresa_id" int NOT NULL,
  "poznamka" varchar(500),
  "penezenka" float(10),

    CONSTRAINT PK_uzivatel_uzivatel_id PRIMARY KEY CLUSTERED (uzivatel_id)
);

CREATE TABLE "adresa" (
  "adresa_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "mesto" varchar(50) NOT NULL,
  "ulice" varchar(50) NOT NULL,
  "PSC" varchar(15),
  "telefon" varchar(15),
  "email" varchar(50) NOT NULL,

  CONSTRAINT PK_adresa_adresa_id PRIMARY KEY CLUSTERED (adresa_id)
);

CREATE TABLE "sledovane" (
  "sledovane_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "uzivatel_id" int NOT NULL,
  "akcie_id" int NOT NULL,

  CONSTRAINT PK_sledovane_sledovane_id PRIMARY KEY CLUSTERED (sledovane_id)
);

CREATE TABLE "pokl_poukazky" (
  "poukazka_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "zkratka" varchar(10) NOT NULL,

  CONSTRAINT PK_pokl_poukazky_poukazka_id PRIMARY KEY CLUSTERED (poukazka_id)
);

CREATE TABLE "historie_poukazek" (
  "poukazka_id" int NOT NULL,
  "datum" datetime NOT NULL,
  "urok" float(10) NOT NULL,

  CONSTRAINT PK_historie_poukazek_poukazka_id_datum PRIMARY KEY CLUSTERED (poukazka_id,datum)
);

CREATE TABLE "portfolio" (
  "portfolio_id" int IDENTITY(1,1) NOT NUll UNIQUE,
  "uzivatel_id" int NOT NULL,
  "akcie_id" int NOT NULL,
  "pocet" int NOT NULL,
  "cena" float(10) NOT NULL,
  "nakup" bit NOT NULL,
  "datum_zmeny" datetime NOT NULL,

  CONSTRAINT PK_portfolio_portfolio_id PRIMARY KEY CLUSTERED (portfolio_id)
);

CREATE TABLE "akcie" (
  "akcie_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "firma_id" int NOT NULL UNIQUE,
  "nazev" varchar(50) NOT NULL,
  "zkratka" varchar(20) NOT NULL,

  CONSTRAINT PK_akcie_akcie_id PRIMARY KEY CLUSTERED (akcie_id)
);

CREATE TABLE "historie_akcie" (
  "historie_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "datum" datetime NOT NULL,
  "akcie_id" int NOT NULL,
  "cena_prodej" float(10) NOT NULL,
  "cena_nakup" float(10) NOT NULL,

  CONSTRAINT PK_historie_akcie_akcie_id_datum PRIMARY KEY CLUSTERED (historie_id,datum)
);

CREATE TABLE "firma" (
  "firma_id" int IDENTITY(1,1) NOT NULL UNIQUE,
  "adresa_id" int NOT NULL UNIQUE,
  "nazev" varchar(30) NOT NULL,
  "ICO" varchar(8),
  "DIC" varchar(10),

  CONSTRAINT PK_firma_firma_id PRIMARY KEY CLUSTERED (firma_id)
);

CREATE TABLE "index_PX" (
  "datum" datetime NOT NULL UNIQUE,
  "hodnota" float(10) NOT NULL,

  CONSTRAINT PK_index_PX_datum PRIMARY KEY CLUSTERED (datum)
);
