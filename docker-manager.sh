#!/bin/bash
# docker-manager.sh - Docker Management Script for aPanel

PROJECT_DIR="/home/app/CPM"
BACKUP_DIR="$PROJECT_DIR/backups"
LOG_DIR="/var/log/docker-app"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Create log directory
mkdir -p $LOG_DIR
mkdir -p $BACKUP_DIR

# Menu
show_menu() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}   Pickleball Club Docker Manager${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
    echo -e "${GREEN}1.${NC}  Show Status"
    echo -e "${GREEN}2.${NC}  Start Services"
    echo -e "${GREEN}3.${NC}  Stop Services"
    echo -e "${GREEN}4.${NC}  Restart Services"
    echo -e "${GREEN}5.${NC}  Restart API Only"
    echo -e "${GREEN}6.${NC}  Restart Database Only"
    echo -e "${GREEN}7.${NC}  View API Logs"
    echo -e "${GREEN}8.${NC}  View Database Logs"
    echo -e "${GREEN}9.${NC}  View All Logs"
    echo -e "${GREEN}10.${NC} Run Database Backup"
    echo -e "${GREEN}11.${NC} Database Shell"
    echo -e "${GREEN}12.${NC} Run Migrations"
    echo -e "${GREEN}13.${NC} Update & Rebuild"
    echo -e "${GREEN}14.${NC} Clean Up System"
    echo -e "${GREEN}15.${NC} Check Disk Space"
    echo -e "${GREEN}16.${NC} Health Check"
    echo -e "${GREEN}17.${NC} View Configuration"
    echo -e "${GREEN}0.${NC}  Exit"
    echo ""
}

# Status function
status() {
    echo -e "${BLUE}Container Status:${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml ps
    echo ""
}

# Start services
start_services() {
    echo -e "${YELLOW}Starting services...${NC}"
    cd $PROJECT_DIR
    docker-compose up -d
    sleep 3
    status
    echo -e "${GREEN}Services started!${NC}"
}

# Stop services
stop_services() {
    echo -e "${YELLOW}Stopping services...${NC}"
    cd $PROJECT_DIR
    docker-compose down
    echo -e "${GREEN}Services stopped!${NC}"
}

# Restart services
restart_services() {
    echo -e "${YELLOW}Restarting services...${NC}"
    cd $PROJECT_DIR
    docker-compose restart
    sleep 3
    status
    echo -e "${GREEN}Services restarted!${NC}"
}

# Restart API
restart_api() {
    echo -e "${YELLOW}Restarting API...${NC}"
    cd $PROJECT_DIR
    docker-compose restart api
    sleep 2
    status
    echo -e "${GREEN}API restarted!${NC}"
}

# Restart Database
restart_db() {
    echo -e "${YELLOW}Restarting Database...${NC}"
    cd $PROJECT_DIR
    docker-compose restart sqlserver
    sleep 3
    status
    echo -e "${GREEN}Database restarted!${NC}"
}

# View API logs
view_api_logs() {
    echo -e "${BLUE}API Logs (Ctrl+C to exit):${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml logs -f api
}

# View database logs
view_db_logs() {
    echo -e "${BLUE}Database Logs (Ctrl+C to exit):${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml logs -f sqlserver
}

# View all logs
view_all_logs() {
    echo -e "${BLUE}All Logs (Ctrl+C to exit):${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml logs -f
}

# Database backup
db_backup() {
    echo -e "${YELLOW}Starting database backup...${NC}"
    DATE=$(date +%Y%m%d_%H%M%S)
    BACKUP_FILE="$BACKUP_DIR/db_backup_$DATE.bak"
    
    docker-compose -f $PROJECT_DIR/docker-compose.yml exec -T sqlserver \
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Hoang416@" \
        -Q "BACKUP DATABASE PickleBallDB_Hoang248 TO DISK = '/var/opt/mssql/backup/db_backup_$DATE.bak'"
    
    docker cp pickleball_sqlserver:/var/opt/mssql/backup/db_backup_$DATE.bak $BACKUP_FILE
    
    if [ -f "$BACKUP_FILE" ]; then
        echo -e "${GREEN}Backup completed: $BACKUP_FILE${NC}"
        ls -lh $BACKUP_FILE
    else
        echo -e "${RED}Backup failed!${NC}"
    fi
}

# Database shell
db_shell() {
    echo -e "${BLUE}Connecting to database...${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml exec sqlserver \
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Hoang416@"
}

# Run migrations
run_migrations() {
    echo -e "${YELLOW}Running Entity Framework migrations...${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml exec api \
        dotnet ef database update
    echo -e "${GREEN}Migrations completed!${NC}"
}

# Update & rebuild
update_rebuild() {
    echo -e "${YELLOW}Pulling latest code from Git...${NC}"
    cd $PROJECT_DIR
    git pull origin main
    
    echo -e "${YELLOW}Rebuilding containers...${NC}"
    docker-compose up -d --build
    
    sleep 3
    status
    echo -e "${GREEN}Update completed!${NC}"
}

# Clean up
cleanup() {
    echo -e "${YELLOW}Cleaning up Docker system...${NC}"
    docker system prune -a -f
    
    echo -e "${YELLOW}Removing old backups (older than 30 days)...${NC}"
    find $BACKUP_DIR -name "*.bak" -mtime +30 -delete
    
    echo -e "${GREEN}Cleanup completed!${NC}"
}

# Check disk space
check_disk() {
    echo -e "${BLUE}Disk Usage:${NC}"
    df -h /
    echo ""
    echo -e "${BLUE}Project Directory Size:${NC}"
    du -sh $PROJECT_DIR
    echo ""
    echo -e "${BLUE}Backups Directory Size:${NC}"
    du -sh $BACKUP_DIR
    echo ""
    echo -e "${BLUE}Docker Images & Volumes:${NC}"
    docker system df
}

# Health check
health_check() {
    echo -e "${BLUE}Performing health check...${NC}"
    echo ""
    
    # Check containers running
    echo -e "${BLUE}Container Status:${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml ps
    echo ""
    
    # Check API health
    echo -e "${BLUE}API Health Endpoint:${NC}"
    curl -s http://localhost:5003/health | python3 -m json.tool || echo "API not responding"
    echo ""
    
    # Check database connection
    echo -e "${BLUE}Database Connection:${NC}"
    docker-compose -f $PROJECT_DIR/docker-compose.yml exec -T sqlserver \
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Hoang416@" \
        -Q "SELECT @@VERSION" > /dev/null 2>&1
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}Database is responding${NC}"
    else
        echo -e "${RED}Database not responding${NC}"
    fi
    echo ""
    
    # Check disk space
    echo -e "${BLUE}Disk Space:${NC}"
    df -h / | tail -1
    echo ""
}

# View configuration
view_config() {
    echo -e "${BLUE}Current Configuration:${NC}"
    echo ""
    docker-compose -f $PROJECT_DIR/docker-compose.yml config | less
}

# Main
while true; do
    show_menu
    read -p "Select option: " choice
    echo ""
    
    case $choice in
        1)  status ;;
        2)  start_services ;;
        3)  stop_services ;;
        4)  restart_services ;;
        5)  restart_api ;;
        6)  restart_db ;;
        7)  view_api_logs ;;
        8)  view_db_logs ;;
        9)  view_all_logs ;;
        10) db_backup ;;
        11) db_shell ;;
        12) run_migrations ;;
        13) update_rebuild ;;
        14) cleanup ;;
        15) check_disk ;;
        16) health_check ;;
        17) view_config ;;
        0)  echo -e "${GREEN}Exiting...${NC}"; exit 0 ;;
        *)  echo -e "${RED}Invalid option!${NC}" ;;
    esac
    
    read -p "Press Enter to continue..."
    clear
done
