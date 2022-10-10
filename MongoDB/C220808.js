db.runCommand({
    collMod: "Questionnaires", 
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["TenantId", "Name", "CreationDateTime", "UserCreator", "IsActive"],
            additionalProperties: false,
            properties: {
                _id: {
                    bsonType: "objectId"
                },
                TenantId: {
                    bsonType: "int",
                    description: "must be a int greater than 0 and is required",
                    minimum: 1
                },
                Name: {
                    bsonType: "string",
                    description: "must be a string and is required",
                    pattern: "^.{3,50}$"
                },
                CreationDateTime: {
                    bsonType: "date",
                    description: "must be a date and is required"
                },
                UserCreator: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                IsActive: {
                    bsonType: "bool",
                    description: "must be a bool and is required"
                },
                Sections: {
                    bsonType: ["array"],
                    minItems: 0,
                    uniqueItems: true,
                    additionalProperties: false,
                    items:
                    {
                        bsonType: "object",
                        required: ["Name", "Order"],
                        additionalProperties: false,
                        properties: {
                            Name: {
                                bsonType: "string",
                                description: "must be a string and is required",
                                pattern: "^.{3,50}$"
                            },
                            Order: {
                                bsonType: "int",
                                description: "must be a int and is required"
                            },
                            IconAF: {
                                bsonType: ["string", "null"],
                                description: "must be a string"
                            },
                            Fields: {
                                bsonType: ["array", "null"],
                                minItems: 0,
                                uniqueItems: true,
                                additionalProperties: false,
                                items:
                                {
                                    bsonType: "object",
                                    required: ["FieldControl", "FieldName", "FieldType", "HasKeyFilter", "IsRequired", "Name", "Order"],
                                    additionalProperties: false,
                                    properties: {
                                        FieldControl: {
                                            bsonType: "int",
                                            description: "must be a int and is required",
                                            enum: [10,11,12,13,14,15,16,21,22,23,30,31,32,40,41,45,50,60,61]
                                        },
                                        FieldName: {
                                            bsonType: "string",
                                            description: "must be a string and is required",
                                            pattern: "^[\\w]{3,50}$"
                                        },
                                        FieldType: {
                                            bsonType: "int",
                                            description: "must be a int and is required",
                                            enum: [1,10,20,30,31,32,40,41,43,50,60,70]
                                        },
                                        FieldSize: {
                                            bsonType: ["int", "null"],
                                            description: "must be a int",
                                        },
                                        HasKeyFilter: {
                                            bsonType: "bool",
                                            description: "must be a bool and is required"
                                        },
                                        InputMask: {
                                            bsonType: ["string", "null"],
                                            description: "must be a string"
                                        },
                                        IsRequired: {
                                            bsonType: "bool",
                                            description: "must be a bool and is required"
                                        },
                                        KeyFilter: {
                                            bsonType: ["string", "null"],
                                            description: "must be a string"
                                        },
                                        Name: {
                                            bsonType: "string",
                                            description: "must be a string and is required",
                                            pattern: "^.{3,50}$"
                                        },
                                        Order: {
                                            bsonType: "int",
                                            description: "must be a int and is required",
                                        },
                                        Options: {
                                            bsonType: ["array", "null"],
                                            minItems: 0,
                                            uniqueItems: true,
                                            additionalProperties: false,
                                            items:
                                            {
                                                bsonType: "object",
                                                required: ["Description", "Value"],
                                                additionalProperties: false,
                                                properties: {
                                                    Description: {
                                                        bsonType: "string",
                                                        description: "must be a string and is required",
                                                        pattern: "^.{1,1000}$"
                                                    },
                                                    Value: {
                                                        bsonType: "int",
                                                        description: "must be a int and is required"
                                                    }
                                                }
                                            }
                                        },
                                        CatalogCustom: {
                                            bsonType: ["object", "null"],
                                            required: ["idCatalogCustom", "FieldName"],
                                            additionalProperties: false,
                                            properties: {
                                                idCatalogCustom: {
                                                    bsonType: "objectId",
                                                    description: "must be a objectId and is a relationship to CatalogsCustom collection"
                                                },
                                                FieldName: {
                                                    bsonType: "string",
                                                    description: "must be a string and is required"
                                                }
                                            }
                                        },
                                        CustomProperties: {
                                            bsonType: ["object", "null"],
                                            additionalProperties: false,
                                            properties: {
                                                Currency: {
                                                    bsonType: ["string", "null"],
                                                    description: "must be a string"
                                                },
                                                Locale: {
                                                    bsonType: ["string", "null"],
                                                    description: "must be a string"
                                                },
                                                MaxValue: {
                                                    bsonType: ["long", "null"],
                                                    description: "must be a long"
                                                },
                                                MinValue: {
                                                    bsonType: ["long", "null"],
                                                    description: "must be a long"
                                                },
                                                UseGrouping: {
                                                    bsonType: ["bool", "null"],
                                                    description: "must be a long"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
});

db.runCommand({
    collMod: "CatalogsCustom", 
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: [
                "CreationDateTime",
                "Description",
                "IsCollectionGenerated",
                "NamePlural",
                "NameSingular",
                "UserCreator",
                "IsActive",
                "Questionnarie"
            ],
            additionalProperties: false,
            properties: {
                _id: {
                    bsonType: "objectId"
                },
                TenantId: {
                    bsonType: "int",
                    description: "must be a int greater than 0 and is required",
                    minimum: 1
                },
                CollectionName: {
                    bsonType: "string",
                    description: "must be a string",
                    pattern: "^[\\w]{3,60}$"
                },
                CreationDateTime: {
                    bsonType: "date",
                    description: "must be a date and is required"
                },
                Description: {
                    bsonType: "string",
                    description: "must be a string and is required",
                    pattern: "^.{3,500}$"
                },
                IsCollectionGenerated: {
                    bsonType: "bool",
                    description: "must be a bool and is required"
                },
                NamePlural: {
                    bsonType: "string",
                    description: "must be a string and is required",
                    pattern: "^.{3,55}$"
                },
                NameSingular: {
                    bsonType: "string",
                    description: "must be a string and is required",
                    pattern: "^.{3,50}$"
                },
                UserCreator: {
                    bsonType: "string",
                    description: "must be a string and is required"
                },
                IsActive: {
                    bsonType: "bool",
                    description: "must be a bool and is required"
                },
                Questionnarie: {
                    bsonType: ["objectId"],
                    description: "must be a objectId and is a relationship to Questionaries collection"
                },
                FieldNames: {
                    bsonType: ["array"],
                    minItems: 0,
                    uniqueItems: true,
                    items:
                    {
                        bsonType: "string",
                        description: "must be a string",
                        pattern: "^[\\w]{3,50}$"
                    }
                }
            }
        }
    },
});