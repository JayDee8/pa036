GO
ALTER TABLE akcie
ADD CONSTRAINT FK_akcie_firma FOREIGN KEY (firma_id) 
    REFERENCES firma (firma_id) 
	ON DELETE NO ACTION
    ON UPDATE NO ACTION
;
GO

GO
ALTER TABLE firma
ADD CONSTRAINT FK_firma_adresa FOREIGN KEY (adresa_id) 
    REFERENCES adresa (adresa_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE uzivatel
ADD CONSTRAINT FK_uzivatel_adresa FOREIGN KEY (adresa_id) 
    REFERENCES adresa (adresa_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE sledovane
ADD CONSTRAINT FK_sledovane_akcie FOREIGN KEY (akcie_id) 
    REFERENCES akcie (akcie_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE sledovane
ADD CONSTRAINT FK_sledovane_uzivatel FOREIGN KEY (uzivatel_id) 
    REFERENCES uzivatel (uzivatel_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE historie_poukazek
ADD CONSTRAINT FK_historie_poukazek_pokl_poukazky FOREIGN KEY (poukazka_id) 
    REFERENCES pokl_poukazky (poukazka_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE portfolio
ADD CONSTRAINT FK_portfolio_uzivatel FOREIGN KEY (uzivatel_id) 
    REFERENCES uzivatel (uzivatel_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE portfolio
ADD CONSTRAINT FK_portfolio_akcie FOREIGN KEY (akcie_id) 
    REFERENCES akcie (akcie_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE historie_akcie
ADD CONSTRAINT FK_historie_akcie_akcie FOREIGN KEY (akcie_id) 
    REFERENCES akcie (akcie_id) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO

GO
ALTER TABLE uzivatel
ADD CONSTRAINT FK_uzivatel_users FOREIGN KEY (uzivatel_id) 
    REFERENCES UserProfile (UserId) 
    ON DELETE CASCADE
    ON UPDATE CASCADE
;
GO