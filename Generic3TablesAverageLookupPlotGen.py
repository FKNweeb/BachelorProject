import pandas as pd
import numpy as np
import os
import mpldatacursor
from matplotlib.backends.backend_pdf import PdfPages
import matplotlib.pyplot as plt


def generate_plot(pdf_pages, csv_filename, table_count, expected_values):
    # Read the CSV file
    data = pd.read_csv(csv_filename)

    X = data['KeyCount'].tolist()
    Y = data['AverageLookup'].tolist()
    table_size_1 = data['TableSize1'].tolist()
    table_size_2 = data['TableSize2'].tolist()
    table_size_3 = data['TableSize3'].tolist()

    fig, ax = plt.subplots()
    scatter = ax.scatter(X, Y, label=f'Actual Average Lookup Time for {table_count} Tables')

    for x, y, size_1, size_2, size_3 in zip(X, Y, table_size_1, table_size_2, table_size_3):
        text = f"table size 1: {size_1}\n table size 2: {size_2}\n table size 3: {size_3}"
        scatter = ax.scatter(x, y, label=text, s=0)

    ax.plot(X, expected_values, label=f'Expected Average Lookup Time for {table_count} Tables', color='red')
    ax.set_xlabel('Number of Keys')
    ax.set_ylabel('Average Lookup Time')
    ax.set_title(f"Average Lookup Time vs Number of Keys for {table_count} Tables")
    ax.grid()
    ax.legend()

    # Enable hover functionality with mpldatacursor
    mpldatacursor.datacursor(hover=True, bbox=dict(fc='w'))

    # Save the current figure to the PDF page
    pdf_pages.savefig(fig)

    dist = np.linalg.norm(np.array(Y) - np.array(expected_values))
    print(dist)


expected_values_dict = {
    3: [2] * 50,
}

pdf_filename = 'Plots/AverageLookupTables.pdf'

with PdfPages(pdf_filename) as pdf_pages:
    for table_count, expected_values in expected_values_dict.items():
        csv_filename = f'CSV_Files/Generic_{table_count}tables.csv'
        generate_plot(pdf_pages, csv_filename, table_count, expected_values)
