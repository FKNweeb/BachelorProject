import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import mplcursors
import os


def generate_plot(csv_filename, table_count, keyCount):
    data = pd.read_csv(csv_filename)

    X = data['AverageLookup'].tolist()
    Y = data['KeyCount'].tolist()

    fig, ax = plt.subplots()
    scatter = ax.scatter(X, Y, label=f'Actual Average Lookup Time for {table_count} Tables')

    ax.set_xlabel('KeyCount')
    ax.set_ylabel('Average Lookup Time')
    ax.set_title(f"Average Lookup Time vs KeyCount {keyCount}, Tables {table_count}")
    ax.grid(True)
    ax.legend()

    # Ensure the Plots directory exists
    avg_dir = 'Plots/AverageLookUp'
    if not os.path.exists(avg_dir):
        os.makedirs(avg_dir)

    plot_filename = os.path.join(f'Plots/AverageLookUp/averageLookUp{keyCount}_{table_count}Tables.png')
    plt.savefig(plot_filename)
    plt.close(fig)


start_table_count = 2
end_table_count = 10
keyCount = [1000, 5000, 10000, 20000, 50000, 100000]

for i in range(start_table_count, end_table_count + 1):
    for j in keyCount:
        csv_filename = f'CSV_Files/AverageLookUp/averageLookUp{j}_{i}tables.csv'
        generate_plot(csv_filename, i, j)