# Stage 1: Build the .NET 7 backend
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-backend
WORKDIR /app
COPY ["source/BudgetManager.Api/BudgetManager.Api.csproj", "source/BudgetManager.Api/"]
COPY ["source/BudgetManager.Application/BudgetManager.Application.csproj", "source/BudgetManager.Application/"]
COPY ["source/BudgetManager.Domain/BudgetManager.Domain.csproj", "source/BudgetManager.Domain/"]
COPY ["source/BudgetManager.Infrastructure/BudgetManager.Infrastructure.csproj", "source/BudgetManager.Infrastructure/"]
RUN dotnet restore source/BudgetManager.Api/BudgetManager.Api.csproj
COPY . .
RUN dotnet publish source/BudgetManager.Api/BudgetManager.Api.csproj -c Release -o /publish

# Stage 2: Build the Vue 3 frontend
FROM node:14 AS build-frontend
WORKDIR /app/frontend
COPY ./source/BudgetManager.UI/package*.json ./
RUN npm install
COPY ./source/BudgetManager.UI/. .
RUN npm run build

# Stage 3: Set up Nginx for frontend and combine with backend
FROM nginx:latest AS final
COPY --from=build-frontend /app/frontend/dist/ /usr/share/nginx/html/
COPY --from=build-backend /publish /usr/share/nginx/html/api/
COPY ./nginx-custom.conf /etc/nginx/conf.d/
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
