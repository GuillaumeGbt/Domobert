CREATE TABLE History_Solar (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Identifiant unique, auto-incrémenté
    Timestamp DATETIME NOT NULL,        -- Horodatage de l'enregistrement
    [Value] DECIMAL(10, 2) NOT NULL,      -- Valeur mesurée (par exemple, en kWh ou autre)
    DeviceID INT NOT NULL,              -- Référence à l'ID du dispositif (foreign key)
    
    CONSTRAINT FK_History_Solar_Devices FOREIGN KEY (DeviceID)
    REFERENCES Devices(Id)
	ON DELETE CASCADE
);

CREATE TABLE History_TempHumi (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Identifiant unique, auto-incrémenté
    Temperature DECIMAL(5, 2) NOT NULL, -- Température avec deux décimales
    Humidity DECIMAL(5, 2) NOT NULL,    -- Humidité avec deux décimales
    Timestamp DATETIME NOT NULL,        -- Horodatage de l'enregistrement
    DeviceID INT NOT NULL,              -- Référence à l'ID du dispositif (clé étrangère)
    
    CONSTRAINT FK_History_TempHumi_Devices FOREIGN KEY (DeviceID)
    REFERENCES Devices(Id)              -- Clé étrangère vers Devices.Id
    ON DELETE CASCADE                   -- Supprime l'historique si le device est supprimé
);