import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import mplcursors
import os


def generate_plot(csv_filename, table_count):
    data = pd.read_csv(csv_filename)

    X = data['HowFilled'].tolist()
    Y = data['AverageLookUp'].tolist()
    table_sizes = [data[f'TableSize{i}'].tolist() for i in range(1, table_count + 1)]

    fig, ax = plt.subplots()
    scatter = ax.scatter(X, Y, label=f'Actual Average Lookup Time for {table_count} Tables')

    labels = [''.join(f"table size {i+1}: {size},\n" for i, size in enumerate(sizes)) for sizes in zip(*table_sizes)]
    mplcursors.cursor(hover=True).connect("add", lambda sel: sel.annotation.set_text(labels[sel.index]))

    ax.set_xlabel('How Filled')
    ax.set_ylabel('Average Lookup Time')
    ax.set_title(f"Average Lookup Time vs How Filled for {table_count} Tables")
    ax.grid(True)
    ax.legend()

    # Ensure the Plots directory exists
    plot_dir = 'Plots'
    if not os.path.exists(plot_dir):
        os.makedirs(plot_dir)

    plot_filename = os.path.join(plot_dir, f'GenericHowFilled_{table_count}_Tables.png')
    plt.savefig(plot_filename)
    plt.show()
    plt.close(fig)


start_table_count = 2
end_table_count = 10

for i in range(start_table_count, end_table_count + 1):
    csv_filename = f'CSV_Files/HowFilled/GenericHowFilled_{i}tables.csv'
    generate_plot(csv_filename, i)
