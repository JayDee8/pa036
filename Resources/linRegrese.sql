ALTER PROCEDURE Lin_regrese
	@ID_Akcie int
AS 
BEGIN
	DECLARE @FETCH_STAT int, @DEBUG Varchar(100)
	DECLARE @LinReg float, @LinReg_Pocit int
	DECLARE @Vynosnost_aktiva real, @Vynosnost_Aktiva_PX real
	DECLARE @P_Datum Datetime, @P_Urok Real
	DECLARE @Datum Datetime, @CProd Real, @CNak Real, @Hodnota Real
	DECLARE @Datum_PRED Datetime, @CProd_PRED Real, @CNak_PRED Real, @Hodnota_PRED Real

	DECLARE Cur_Hist_Pouk CURSOR FOR
		SELECT Datum, Urok FROM Historie_Poukazek ORDER BY Datum
	DECLARE Cur_AkcInd CURSOR FOR
		SELECT Index_PX.Datum, Cena_prodej, Cena_nakup, Hodnota FROM historie_akcie, Index_PX 
			WHERE Akcie_Id = @ID_Akcie AND Historie_Akcie.Datum = Index_PX.Datum ORDER BY Index_PX.Datum
	OPEN Cur_Hist_pouk
	OPEN Cur_AkcInd

	FETCH NEXT FROM Cur_Hist_pouk INTO @P_Datum, @P_Urok
	FETCH NEXT FROM Cur_AkcInd INTO @Datum_PRED, @CProd_PRED, @CNak_PRED, @Hodnota_PRED
	FETCH NEXT FROM Cur_AkcInd INTO @Datum, @CProd, @CNak, @Hodnota
	SET @LinREG_Pocit = 0
	SET @LinReg = 0

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
--		Výpoèet Výnosnosti Aktív z Histórie Akcií
		SET @Vynosnost_aktiva = (@CProd + @CNak)/2 - (@CProd_PRED + @CNak_PRED) / 2
		SET @Vynosnost_aktiva = @Vynosnost_aktiva / (@CProd_PRED + @CNak_PRED) / 2 --* 100
		SET @Vynosnost_aktiva = @Vynosnost_aktiva - 0.01* @P_Urok
--		Výpoèet Výnosnosti aktív_PX z Index_PX
		SET @Vynosnost_Aktiva_PX = (@Hodnota - @Hodnota_PRED) / @Hodnota_PRED --* 100
		SET @Vynosnost_aktiva_PX = @Vynosnost_aktiva_PX - 0.01* @P_Urok
--		!!!!!!!!!!!!!!!!!!  Teraz Èerešnièka - LINEARNI REGRESE
		SET @LinReg = @linReg + @Vynosnost_Aktiva / @Vynosnost_Aktiva_PX 

--		##############################  TLAÈÍME
/*		SET @debug = convert(varchar,@Vynosnost_aktiva) + '  ' + convert(varchar,@Vynosnost_Aktiva_PX)
		SET @debug = @debug + ' * ' + convert(varchar,@LinReg)
		SET @debug = @debug + ' | ' + convert(varchar,@Datum, 104) + '  \  ' + convert(varchar,@LinReg_Pocit)
		Print @debug
*/
--		Prepis premenný - posun o krok
		SET @Datum_PRED = @Datum
		SET @CProd_PRED = @CProd
		SET @CNak_PRED = @Cnak
		SET @Hodnota_PRED = @Hodnota
		SET @LinReg_pocit = @LinReg_pocit + 1

--		Ak sme sa pohli do nového mesiaca, daj nový úrok
		IF (@Datum_PRED < @P_Datum)
			FETCH NEXT FROM Cur_Hist_pouk INTO @P_Datum, @P_Urok
			
--		Daj Ïalší riadok Z Histórie Akcií
		FETCH NEXT FROM Cur_AkcInd INTO @Datum, @CProd, @CNak, @Hodnota
	
	END

--Print ' '
--Print @LinReg / @linReg_Pocit

CLOSE Cur_Hist_Pouk
CLOSE Cur_AkcInd

DEALLOCATE Cur_Hist_Pouk
DEALLOCATE Cur_AkcInd

select (@LinReg / @linReg_Pocit - 1 ) * 100 as LinReg_OUT 

END
