CREATE TABLE History_Solar (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Identifiant unique, auto-incr�ment�
    Timestamp DATETIME NOT NULL,        -- Horodatage de l'enregistrement
    [Value] DECIMAL(10, 2) NOT NULL,      -- Valeur mesur�e (par exemple, en kWh ou autre)
    DeviceID INT NOT NULL,              -- R�f�rence � l'ID du dispositif (foreign key)
    
    CONSTRAINT FK_History_Solar_Devices FOREIGN KEY (DeviceID)
    REFERENCES Devices(Id)
	ON DELETE CASCADE
);

CREATE TABLE History_TempHumi (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Identifiant unique, auto-incr�ment�
    Temperature DECIMAL(5, 2) NOT NULL, -- Temp�rature avec deux d�cimales
    Humidity DECIMAL(5, 2) NOT NULL,    -- Humidit� avec deux d�cimales
    Timestamp DATETIME NOT NULL,        -- Horodatage de l'enregistrement
    DeviceID INT NOT NULL,              -- R�f�rence � l'ID du dispositif (cl� �trang�re)
    
    CONSTRAINT FK_History_TempHumi_Devices FOREIGN KEY (DeviceID)
    REFERENCES Devices(Id)              -- Cl� �trang�re vers Devices.Id
    ON DELETE CASCADE                   -- Supprime l'historique si le device est supprim�
);