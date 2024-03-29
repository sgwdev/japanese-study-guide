﻿using Core;
using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void UseSnakeCaseConvention(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                string tableName = entity.GetTableName().ToSnakeCase();
                entity.SetTableName(tableName);

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName(StoreObjectIdentifier.Table(tableName, null)).ToSnakeCase());
                }

                // Replace primary key names
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                // Replace foreign key names
                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                }

                // Replace index names
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }
        }

        public static void Seed(this ModelBuilder builder)
        {
            ReadingType onReading = new ReadingType { Id = Constants.ReadingTypes.On, Label = "On" };
            ReadingType kunReading = new ReadingType { Id = Constants.ReadingTypes.Kun, Label = "Kun" };
            ReadingType specialReading = new ReadingType { Id = Constants.ReadingTypes.Special, Label = "Special" };

            builder.Entity<ReadingType>().HasData(
                onReading,
                kunReading,
                specialReading
            );
        }
    }
}
