ALTER PROCEDURE Lin_regrese
	@ID_Akcie int
AS 
BEGIN
	DECLARE @FETCH_STAT int, @DEBUG Varchar(100)
	DECLARE @LinReg float, @LinReg_Pocit int
	DECLARE @Vynosnost_aktiva real, @Vynosnost_Aktiva_PX real
	DECLARE @P_Datum Datetime, @Urok Real
	DECLARE @A_Datum Datetime, @CProd Real, @CNak Real
	DECLARE @I_Datum Datetime, @Hodnota Real
	DECLARE @A_Datum_PRED Datetime, @CProd_PRED Real, @CNak_PRED Real
	DECLARE @I_Datum_PRED Datetime, @Hodnota_PRED Real

	DECLARE Cur_Hist_Akcie CURSOR FOR
		SELECT Datum, Cena_prodej, Cena_nakup FROM Historie_Akcie 
			WHERE Akcie_ID = @ID_Akcie ORDER BY Datum
	OPEN Cur_Hist_Akcie
	FETCH NEXT FROM Cur_Hist_Akcie INTO @A_Datum, @CProd, @CNak

	DECLARE Cur_Index_PX CURSOR FOR
		SELECT Hodnota FROM Index_PX WHERE Datum = @A_Datum
	OPEN Cur_Index_PX
	FETCH NEXT FROM Cur_Index_PX INTO @Hodnota

--	###########################  H¾adanie 1. vety v Index_PX s Datum = Hist_Akcie.Datum
	WHILE (@@FETCH_STATUS <>0)
	BEGIN
		CLOSE Cur_Index_PX
		DEALLOCATE Cur_Index_PX
		FETCH NEXT FROM Cur_Hist_Akcie INTO @A_Datum, @CProd, @CNak
		DECLARE Cur_Index_PX CURSOR FOR
			SELECT Hodnota FROM Index_PX WHERE Datum = @A_Datum
		OPEN Cur_Index_PX
		FETCH NEXT FROM Cur_Index_PX INTO @Hodnota
	END
	CLOSE Cur_Index_PX
	DEALLOCATE Cur_Index_PX

--	########################### Zistenie správneho úroku pod¾a dátumu
	DECLARE Cur_Hist_Pouk CURSOR FOR		-- Najprv najvhodnejšieho dátumu
		SELECT Datum, Urok FROM Historie_Poukazek WHERE Datum = (
			SELECT MAX(Datum) FROM Historie_Poukazek WHERE Datum < @A_Datum) 
	OPEN Cur_Hist_pouk
	FETCH NEXT FROM Cur_Hist_pouk INTO @P_Datum, @Urok
	CLOSE Cur_Hist_pouk
	DEALLOCATE Cur_Hist_pouk
	DECLARE Cur_Hist_Pouk CURSOR FOR		-- Definícia nového Kurzora
		SELECT Datum, Urok FROM Historie_Poukazek WHERE Datum >=@P_Datum ORDER BY Datum
	OPEN Cur_Hist_pouk
	FETCH NEXT FROM Cur_Hist_pouk INTO @P_Datum, @Urok

	PRINT '*'
	print convert(Varchar(20),@A_Datum) + '  ' + convert(Varchar(10),@Hodnota)

--	######################## Naplnenie Hodnôt -1. kroku
	SET @A_Datum_PRED = @A_Datum
	SET @CProd_PRED = @CProd
	SET @CNak_PRED = @CNak
	SET @Hodnota_PRED = @Hodnota

--	#########################  Hlavný cyklus Pre všetky záznamy z histórie Akcii
	SET @LinREG_Pocit = 0
	SET @LinReg = 0

	FETCH NEXT FROM Cur_Hist_Akcie INTO @A_Datum, @CProd, @CNak
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
--		################## Zisti, èi je veta s rovnakým dátumom v Index_PX
		DECLARE Cur_Index_PX CURSOR FOR
			SELECT Hodnota FROM Index_PX WHERE Datum = @A_Datum
		OPEN Cur_Index_PX
		FETCH NEXT FROM Cur_Index_PX INTO @Hodnota
		CLOSE Cur_Index_PX
		DEALLOCATE Cur_Index_PX
		IF ( @@FETCH_STATUS <> 0 )
		BEGIN		-- Nenašla sa vhodná veta v Index_PX - skús znova
			FETCH NEXT FROM Cur_Hist_Akcie INTO @A_Datum, @CProd, @CNak
			CONTINUE
		END
		ELSE		-- Našla sa veta - poèítaj
		BEGIN
--			Výpoèet Výnosnosti Aktív z Histórie Akcií
			SET @Vynosnost_aktiva = (@CProd + @CNak)/2 - (@CProd_PRED + @CNak_PRED) / 2
			SET @Vynosnost_aktiva = @Vynosnost_aktiva / (@CProd_PRED + @CNak_PRED) / 2 
			SET @Vynosnost_aktiva = @Vynosnost_aktiva - 0.01* @Urok
--			Výpoèet Výnosnosti aktív_PX z Index_PX
			SET @Vynosnost_Aktiva_PX = (@Hodnota - @Hodnota_PRED) / @Hodnota_PRED 
			SET @Vynosnost_aktiva_PX = @Vynosnost_aktiva_PX - 0.01* @Urok
--			!!!!!!!!!!!!!!!!!!  Teraz Èerešnièka - LINEARNI REGRESE
			SET @LinReg = @linReg + abs(@Vynosnost_Aktiva / @Vynosnost_Aktiva_PX)
/*
--			##############################  TLAÈÍME
			SET @debug = convert(varchar,@Vynosnost_aktiva) + '  ' + convert(varchar,@Vynosnost_Aktiva_PX)
			SET @debug = @debug + '                *               ' + convert(varchar,abs(@Vynosnost_Aktiva / @Vynosnost_Aktiva_PX ))
--			SET @debug = @debug + ' | ' + convert(varchar,@A_Datum, 104) + '  \  ' + convert(varchar,@LinReg_Pocit)
--			SET @debug = @debug + ' | ' + convert(varchar,@P_Datum, 104) + '  /  ' + convert(varchar,@Urok)

			Print @debug
*/
--			Prepis premenný - posun o krok
			SET @A_Datum_PRED = @A_Datum
			SET @CProd_PRED = @CProd
			SET @CNak_PRED = @Cnak
			SET @Hodnota_PRED = @Hodnota
			SET @LinReg_pocit = @LinReg_pocit + 1

--			Ak sme sa pohli do nového mesiaca, daj nový úrok
			IF (@A_Datum_PRED > @P_Datum)
				FETCH NEXT FROM Cur_Hist_pouk INTO @P_Datum, @Urok
			
--			Daj Ïalší riadok Z Histórie Akcií
			FETCH NEXT FROM Cur_Hist_Akcie INTO @A_Datum, @CProd, @CNak
		END
	END

--Print ' '
--Print @LinReg / @linReg_Pocit

CLOSE Cur_Hist_Pouk
CLOSE Cur_Hist_Akcie

DEALLOCATE Cur_Hist_Pouk
DEALLOCATE Cur_Hist_Akcie

select (@LinReg / @linReg_Pocit )  as LinReg_OUT 
/*
IF (SELECT COUNT(*) FROM Linearni_Regrese WHERE Akcie_ID = @ID_Akcie) = 0
	INSERT INTO Linearni_Regrese VALUES(@ID_Akcie, (@LinReg / @linReg_Pocit - 1 ) * 100)
ELSE
	UPDATE Linearni_Regrese SET Linearni_Regrese = (@LinReg / @linReg_Pocit - 1 ) * 100
		WHERE Akcie_ID = @ID_Akcie
*/
END
